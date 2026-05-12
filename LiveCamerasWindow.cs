using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EventPassMX_programacion
{
    public class LiveCamerasWindow : Window
    {
        private string usuario;
        private ComboBox cbTickets;

        public LiveCamerasWindow(string usuario)
        {
            this.usuario = usuario;
            this.Title = "Cámaras en vivo";
            this.Width = 600;
            this.Height = 400;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            
            var grid = new Grid
            {
                Background = (Brush)Application.Current.Resources["AppBackground"]
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(70) });
            grid.RowDefinitions.Add(new RowDefinition()); 

            
            var header = new Border
            {
                Background = (Brush)Application.Current.Resources["HeaderGradient"],
                Child = new TextBlock
                {
                    Text = "Cámaras en vivo",
                    Foreground = Brushes.White,
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };

            Grid.SetRow(header, 0);
            grid.Children.Add(header);

            
            var card = new Border
            {
                Background = (Brush)Application.Current.Resources["CardBackground"],
                CornerRadius = new CornerRadius(15),
                Margin = new Thickness(20),
                Padding = new Thickness(15)
            };

            Grid.SetRow(card, 1);

            var content = new StackPanel();

            content.Children.Add(new TextBlock
            {
                Text = "Selecciona un ticket para ver la transmisión",
                Foreground = Brushes.White,
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 10)
            });

            cbTickets = new ComboBox
            {
                Height = 35,
                Margin = new Thickness(0, 0, 0, 10)
            };

            content.Children.Add(cbTickets);

            
            var btnView = new Button
            {
                Content = "VER CÁMARAS",
                Style = (Style)Application.Current.Resources["BuyButton"],
                Margin = new Thickness(0, 10, 0, 0)
            };

            btnView.Click += BtnView_Click;
            content.Children.Add(btnView);

          
            var btnClose = new Button
            {
                Content = "Cerrar",
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 80
            };

            btnClose.Click += (s, e) => this.Close();
            content.Children.Add(btnClose);

            card.Child = content;
            grid.Children.Add(card);

            this.Content = grid;

            LoadTickets();
        }

        private void LoadTickets()
        {
            cbTickets.Items.Clear();
            var my = DataStore.GetTicketsForUser(usuario);

            foreach (var t in my)
            {
                cbTickets.Items.Add(new ComboBoxItem
                {
                    Content = $"{t.Evento.Nombre} - {t.Access}",
                    Tag = t.Id
                });
            }
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            if (cbTickets.SelectedItem is ComboBoxItem it && it.Tag is System.Guid id)
            {
                var ticket = DataStore.GetTicketsForUser(usuario)
                    .FirstOrDefault(t => t.Id == id);

                if (ticket == null)
                {
                    MessageBox.Show("Ticket no encontrado.");
                    return;
                }

                MessageBox.Show($"Transmisión en vivo:\n{ticket.Evento.Nombre}\nAcceso: {ticket.Access}");
            }
            else
            {
                MessageBox.Show("Selecciona un ticket.");
            }
        }
    }
}