using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EventPassMX_programacion
{
    public class RewardsWindow : Window
    {
        private string usuario;
        private TextBlock txtPoints;

        public RewardsWindow(string usuario)
        {
            this.usuario = usuario;
            this.Title = "Recompensas";
            this.Width = 420;
            this.Height = 260;

            Brush bg = new SolidColorBrush(Color.FromRgb(30, 30, 50));
            Brush accent = new SolidColorBrush(Color.FromRgb(140, 100, 255));
            Brush textColor = new SolidColorBrush(Color.FromRgb(230, 230, 255));

            this.Background = bg;

            var grid = new Grid { Margin = new Thickness(15) };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var title = new TextBlock
            {
                Text = "Puntos y recompensas",
                Foreground = textColor,
                FontSize = 20,
                FontWeight = FontWeights.Bold
            };

            txtPoints = new TextBlock
            {
                Foreground = textColor,
                Margin = new Thickness(0, 10, 0, 0)
            };

            var btn = new Button
            {
                Content = "Canjear 250 pts",
                Background = accent,
                Foreground = Brushes.White,
                Height = 35
            };

            btn.Click += BtnRedeem_Click;

            Grid.SetRow(title, 0);
            Grid.SetRow(txtPoints, 1);
            Grid.SetRow(btn, 2);

            grid.Children.Add(title);
            grid.Children.Add(txtPoints);
            grid.Children.Add(btn);

            this.Content = grid;

            Refresh();
        }

        private void Refresh()
        {
            var pts = DataStore.GetPoints(usuario);
            txtPoints.Text = $"Puntos acumulados: {pts}\n\nRegla: $200 = 1 punto. Ejemplo canje: 250 puntos = $100 de descuento.";
        }

        private void BtnRedeem_Click(object sender, RoutedEventArgs e)
        {
            var ok = DataStore.RedeemPoints(usuario, 250);
            if (ok)
            {
                MessageBox.Show("Canje realizado. Ahora puedes usar el descuento en tu próxima compra.", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
                Refresh();
            }
            else
            {
                MessageBox.Show("No tienes suficientes puntos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
