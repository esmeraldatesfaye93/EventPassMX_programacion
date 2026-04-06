using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EventPassMX_programacion
{
    public class VIPWindow : Window
    {
        private string usuario;
        private ComboBox cbEvents;

        public VIPWindow(string usuario)
        {
            this.usuario = usuario;
            this.Title = "Experiencia VIP";
            this.Width = 520;
            this.Height = 340;

            // 🎨 COLORES
            Brush bg = new SolidColorBrush(Color.FromRgb(30, 30, 50));
            Brush primary = new SolidColorBrush(Color.FromRgb(55, 55, 80));
            Brush accent = new SolidColorBrush(Color.FromRgb(140, 100, 255));
            Brush textColor = new SolidColorBrush(Color.FromRgb(230, 230, 255));

            this.Background = bg;

            var grid = new Grid { Margin = new Thickness(15) };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var title = new TextBlock
            {
                Text = "Experiencia VIP / Meet & Greet",
                Foreground = textColor,
                FontSize = 20,
                FontWeight = FontWeights.Bold
            };

            cbEvents = new ComboBox
            {
                Background = primary,
                Foreground = textColor,
                Height = 35,
                Margin = new Thickness(0, 10, 0, 10)
            };

            foreach (var e in DataStore.Eventos)
            {
                cbEvents.Items.Add(new ComboBoxItem
                {
                    Content = e.Nombre,
                    Tag = e,
                    Foreground = Brushes.Black
                });
            }

            var btnBuy = new Button
            {
                Content = "Comprar VIP",
                Background = accent,
                Foreground = Brushes.White,
                Height = 35
            };
            btnBuy.Click += BtnBuyVip_Click;

            Grid.SetRow(title, 0);
            Grid.SetRow(cbEvents, 1);
            Grid.SetRow(btnBuy, 2);

            grid.Children.Add(title);
            grid.Children.Add(cbEvents);
            grid.Children.Add(btnBuy);

            this.Content = grid;
        }

        private void BtnBuyVip_Click(object sender, RoutedEventArgs e)
        {
            if (cbEvents.SelectedItem is ComboBoxItem it && it.Tag is Evento ev)
            {
                var pkg = new DataStore.VIPPackage
                {
                    Name = "VIP Básico",
                    Price = ev.Precio * 0.3m,
                    Description = "Meet & Greet digital + contenido VIP"
                };

                var t = DataStore.PurchaseVIP(usuario, ev, pkg);

                if (t != null)
                {
                    MessageBox.Show($"Paquete VIP comprado. Ticket VIP generado (ID: {t.Id}).",
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo completar la compra VIP.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un evento.",
                    "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}