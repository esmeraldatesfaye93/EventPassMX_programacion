using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EventPassMX_programacion
{
    public class VotingWindow : Window
    {
        private ComboBox cbEvents;
        private ListBox lbOptions;
        private TextBox txtOption;

        public VotingWindow()
        {
            this.Title = "Votación";
            this.Width = 600;
            this.Height = 420;

            Brush bg = new SolidColorBrush(Color.FromRgb(30, 30, 50));
            Brush primary = new SolidColorBrush(Color.FromRgb(55, 55, 80));
            Brush accent = new SolidColorBrush(Color.FromRgb(140, 100, 255));
            Brush textColor = new SolidColorBrush(Color.FromRgb(230, 230, 255));

            this.Background = bg;

            var grid = new Grid { Margin = new Thickness(15) };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            cbEvents = new ComboBox
            {
                Background = primary,
                Foreground = textColor,
                Height = 35
            };

            foreach (var e in DataStore.Eventos)
            {
                cbEvents.Items.Add(new ComboBoxItem { Content = e.Nombre, Tag = e.Nombre, Foreground = Brushes.Black });
            }

            cbEvents.SelectionChanged += (s, e) => LoadVotes();

            lbOptions = new ListBox
            {
                Background = primary,
                Foreground = textColor
            };

            var panel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 10, 0, 0) };

            txtOption = new TextBox { Width = 250 };

            var btnAdd = new Button
            {
                Content = "Agregar",
                Background = accent,
                Foreground = Brushes.White,
                Margin = new Thickness(5, 0, 0, 0)
            };

            var btnVote = new Button
            {
                Content = "Votar",
                Background = accent,
                Foreground = Brushes.White,
                Margin = new Thickness(5, 0, 0, 0)
            };

            btnAdd.Click += BtnAdd_Click;
            btnVote.Click += BtnVote_Click;

            panel.Children.Add(txtOption);
            panel.Children.Add(btnAdd);
            panel.Children.Add(btnVote);

            Grid.SetRow(cbEvents, 0);
            Grid.SetRow(lbOptions, 1);
            Grid.SetRow(panel, 2);

            grid.Children.Add(cbEvents);
            grid.Children.Add(lbOptions);
            grid.Children.Add(panel);

            this.Content = grid;
        }


        private void LoadVotes()
        {
            lbOptions.Items.Clear();
            if (cbEvents.SelectedItem is ComboBoxItem it && it.Tag is string ev)
            {
                var votes = DataStore.GetVotesForEvent(ev);
                foreach (var v in votes)
                {
                    lbOptions.Items.Add(new ListBoxItem { Content = $"{v.OptionName} - {v.Votes} votos", Tag = v.OptionId });
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!(cbEvents.SelectedItem is ComboBoxItem it && it.Tag is string ev))
            {
                MessageBox.Show("Seleccione un evento.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtOption.Text))
            {
                MessageBox.Show("Ingrese una opción.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var id = System.Guid.NewGuid().ToString();
            DataStore.VoteForOption(ev, id, txtOption.Text.Trim());
            txtOption.Clear();
            LoadVotes();
        }

        private void BtnVote_Click(object sender, RoutedEventArgs e)
        {
            if (!(cbEvents.SelectedItem is ComboBoxItem it && it.Tag is string ev))
            {
                MessageBox.Show("Seleccione un evento.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (lbOptions.SelectedItem is ListBoxItem li && li.Tag is string optId)
            {
                var optName = li.Content.ToString();
                DataStore.VoteForOption(ev, optId, optName);
                LoadVotes();
            }
            else
            {
                MessageBox.Show("Seleccione una opción para votar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
