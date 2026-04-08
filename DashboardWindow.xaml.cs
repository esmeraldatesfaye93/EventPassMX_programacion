using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace EventPassMX_programacion
{
    public class DashboardWindow : Window
    {
        private string usuario;
        private WrapPanel panelEventos;
        private TextBlock txtDetails;
        private Evento eventoSeleccionado;

        public DashboardWindow(string user)
        {
            usuario = user;
            panelEventos = new WrapPanel();
            Title = "EventPassMX";
            Width = 1100;
            Height = 700;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;

            // 🎨 COLORES
            var bg = new SolidColorBrush(Color.FromRgb(15, 15, 30));
            var card = new SolidColorBrush(Color.FromRgb(30, 30, 60));
            var accent = new SolidColorBrush(Color.FromRgb(120, 90, 255));
            var text = Brushes.White;

            Background = bg;

            var root = new DockPanel();

            // 🔝 HEADER
            var header = new Grid { Height = 60, Background = card };
            header.ColumnDefinitions.Add(new ColumnDefinition());
            header.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var title = new TextBlock
            {
                Text = $"🎟 EventPassMX | {usuario}",
                Foreground = text,
                FontSize = 18,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(15, 0, 0, 0)
            };

            var btnLogout = new Button
            {
                Content = "Cerrar sesión",
                Background = Brushes.Red,
                Foreground = Brushes.White,
                Margin = new Thickness(10),
                Padding = new Thickness(10, 5, 10, 5),
                BorderThickness = new Thickness(0)
            };
            btnLogout.Click += (s, e) => this.Close();

            Grid.SetColumn(title, 0);
            Grid.SetColumn(btnLogout, 1);

            header.Children.Add(title);
            header.Children.Add(btnLogout);

            DockPanel.SetDock(header, Dock.Top);
            root.Children.Add(header);

            // 🧱 GRID PRINCIPAL
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(320) });

            // 🎫 EVENTOS (cards)
            panelEventos = new WrapPanel { Margin = new Thickness(15) };

            var scroll = new ScrollViewer
            {
                Content = panelEventos,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            Grid.SetColumn(scroll, 0);
            grid.Children.Add(scroll);

            // 📄 PANEL DERECHO
            var right = new StackPanel
            {
                Background = card,
                Margin = new Thickness(10),
                
            };

            right.Children.Add(new TextBlock
            {
                Text = "Detalles del evento",
                Foreground = text,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            });

            txtDetails = new TextBlock
            {
                Foreground = text,
                Margin = new Thickness(0, 10, 0, 10),
                TextWrapping = TextWrapping.Wrap
            };

            right.Children.Add(txtDetails);

            var btnBuy = new Button
            {
                Content = "Comprar boleto",
                Background = accent,
                Foreground = Brushes.White,
                Padding = new Thickness(10),
                BorderThickness = new Thickness(0)
            };

            btnBuy.Click += BtnBuy_Click;
            right.Children.Add(btnBuy);

            Grid.SetColumn(right, 1);
            grid.Children.Add(right);

            root.Children.Add(grid);
            Content = root;

            LoadEvents(card, accent, text);
        }

        // 🎫 CREAR TARJETAS
        private void LoadEvents(Brush card, Brush accent, Brush text)
        {
            panelEventos.Children.Clear();

            foreach (var ev in DataStore.Eventos)
            {
                var border = new Border
                {
                    Width = 220,
                    Height = 160,
                    Background = card,
                    Margin = new Thickness(10),
                    CornerRadius = new CornerRadius(12),
                    Cursor = Cursors.Hand
                };

                var stack = new StackPanel { Margin = new Thickness(10) };

                stack.Children.Add(new TextBlock
                {
                    Text = ev.Nombre,
                    Foreground = text,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14
                });

                stack.Children.Add(new TextBlock
                {
                    Text = $"💲 {ev.Precio}",
                    Foreground = accent,
                    Margin = new Thickness(0, 5, 0, 0)
                });

                border.Child = stack;

                // CLICK
                border.MouseLeftButtonUp += (s, e) =>
                {
                    eventoSeleccionado = ev;
                    txtDetails.Text = $"🎤 {ev.Nombre}\n💲 Precio: ${ev.Precio}\n📅 Fecha: Próximamente";
                };

                // HOVER
                border.MouseEnter += (s, e) =>
                {
                    border.Background = new SolidColorBrush(Color.FromRgb(60, 60, 120));
                };

                border.MouseLeave += (s, e) =>
                {
                    border.Background = card;
                };

                panelEventos.Children.Add(border);
            }
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            if (eventoSeleccionado == null)
            {
                MessageBox.Show("Selecciona un evento primero.");
                return;
            }

            MessageBox.Show($"Compra realizada para {eventoSeleccionado.Nombre} 🎟️");
        }
    }
}