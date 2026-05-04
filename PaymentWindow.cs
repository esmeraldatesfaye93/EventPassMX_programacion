using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace EventPassMX_programacion
{
    public class PaymentWindow : Window
    {
        private string usuario;
        private Evento evento;
        private AccessLevel tipo;

        private TextBox txtCard;
        private TextBox txtName;

        public PaymentWindow(string user, Evento ev, AccessLevel t)
        {
            usuario = user;
            evento = ev;
            tipo = t;

            this.Title = "Pago Seguro";
            this.Width = 400;
            this.Height = 330;
            this.Background = new SolidColorBrush(Color.FromRgb(20, 20, 35));

            var stack = new StackPanel { Margin = new Thickness(20) };

            stack.Children.Add(new TextBlock { Text = $"Evento: {evento.Nombre}", Foreground = Brushes.White });
            stack.Children.Add(new TextBlock { Text = $"Tipo: {tipo}", Foreground = Brushes.Gray });
            stack.Children.Add(new TextBlock
            {
                Text = $"Total: ${evento.Precio}",
                Foreground = Brushes.LightGreen,
                FontSize = 16
            });

            stack.Children.Add(new TextBlock { Text = "Número de tarjeta", Foreground = Brushes.White });

            txtCard = new TextBox { Height = 30 };
            stack.Children.Add(txtCard);

            stack.Children.Add(new TextBlock { Text = "Nombre en tarjeta", Foreground = Brushes.White });

            txtName = new TextBox { Height = 30 };
            stack.Children.Add(txtName);

            var btnPagar = new Button
            {
                Content = "Pagar ahora",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                Height = 40,
                Margin = new Thickness(0, 15, 0, 0)
            };

            btnPagar.Click += BtnPagar_Click;
            stack.Children.Add(btnPagar);

            this.Content = stack;
        }

        private void BtnPagar_Click(object sender, RoutedEventArgs e)
        {
            

            Ticket ticket = null;

            if (tipo == AccessLevel.General)
            {
                ticket = DataStore.CreateTicket(usuario, evento);
            }
            else
            {
                var pkg = new DataStore.VIPPackage
                {
                    Name = tipo.ToString(),
                    Price = evento.Precio * 0.3m,
                    Description = "VIP"
                };

                ticket = DataStore.PurchaseVIP(usuario, evento, pkg);

                if (ticket != null)
                    ticket.Access = tipo;
            }

            if (ticket != null)
            {
                var path = GeneradorBoletos.GeneratePDF(ticket);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });

                MessageBox.Show($"Pago exitoso\nFolio: {ticket.QRCode}");

                new HistorialWindow(usuario).Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Error en el pago");
            }
        }
    }
}