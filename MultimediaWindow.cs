using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EventPassMX_programacion
{
    public class MultimediaWindow : Window
    {
        private string usuario;
        private ComboBox cbEvents;
        private ListBox lbItems;
        private TextBox txtUrl;
        private CheckBox chkVip;

        public MultimediaWindow(string usuario)
        {
            this.usuario = usuario;
            this.Title = "Multimedia";
            this.Width = 700;
            this.Height = 450;
            this.Background = new SolidColorBrush(Color.FromRgb(18, 18, 30));

            var grid = new Grid { Margin = new Thickness(15) };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            cbEvents = new ComboBox();
            Grid.SetRow(cbEvents, 0);

            lbItems = new ListBox { Background = Brushes.Black, Foreground = Brushes.White };
            Grid.SetRow(lbItems, 1);

            var panel = new StackPanel { Orientation = Orientation.Horizontal };
            txtUrl = new TextBox { Width = 300 };
            chkVip = new CheckBox { Content = "VIP" };
            var btn = new Button { Content = "Agregar" };
            btn.Click += BtnAdd_Click;

            panel.Children.Add(txtUrl);
            panel.Children.Add(chkVip);
            panel.Children.Add(btn);

            Grid.SetRow(panel, 2);

            grid.Children.Add(cbEvents);
            grid.Children.Add(lbItems);
            grid.Children.Add(panel);

            this.Content = grid;
        }

        private void LoadItems()
        {
            lbItems.Items.Clear();
            if (cbEvents.SelectedItem is ComboBoxItem it && it.Tag is string ev)
            {
                var items = DataStore.GetMultimediaForEvent(ev);
                foreach (var m in items)
                {
                    lbItems.Items.Add($"{m.Type}: {m.Url} {(m.IsVipOnly ? "(VIP)" : "")}");
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
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
            {
                MessageBox.Show("Ingrese la URL del recurso.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var item = new MultimediaItem { EventName = ev, Type = "Foto/Video", Url = txtUrl.Text.Trim(), IsVipOnly = chkVip.IsChecked == true };
            DataStore.AddMultimedia(item);
            MessageBox.Show("Multimedia agregada (simulado).", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadItems();
            txtUrl.Clear();
            chkVip.IsChecked = false;
        }
    }
}
