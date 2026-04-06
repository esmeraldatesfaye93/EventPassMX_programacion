using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EventPassMX_programacion
{
    public class AccessWindow : Window
    {
        private string usuario;
        private ListBox lbTickets;

        public AccessWindow(string usuario)
        {
            this.usuario = usuario;
            this.Title = "Accesos";
            this.Width = 600;
            this.Height = 400;
            this.Background = new SolidColorBrush(Color.FromRgb(18, 18, 30));

            var grid = new Grid { Margin = new Thickness(15) };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var title = new TextBlock
            {
                Text = "Accesos por boleto",
                Foreground = Brushes.White,
                FontSize = 20
            };
            Grid.SetRow(title, 0);
            grid.Children.Add(title);

            lbTickets = new ListBox { Background = Brushes.Black, Foreground = Brushes.White };
            Grid.SetRow(lbTickets, 1);
            grid.Children.Add(lbTickets);

            var btn = new Button { Content = "Cerrar", HorizontalAlignment = HorizontalAlignment.Right };
            btn.Click += (s, e) => this.Close();
            Grid.SetRow(btn, 2);
            grid.Children.Add(btn);

            this.Content = grid;

            LoadTickets();
        }

        private void LoadTickets()
        {
            lbTickets.Items.Clear();
            var tickets = DataStore.GetTicketsForUser(usuario);
            foreach (var t in tickets)
            {
                lbTickets.Items.Add($"Evento: {t.Evento.Nombre} - Acceso: {t.Access} - QR: {t.QRCode} - Comprado: {t.FechaCompra}");
            }
        }
    }
}
