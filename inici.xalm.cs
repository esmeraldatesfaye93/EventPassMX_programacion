using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace EventPassMX_programacion
{
    public class Inicio : Window
    {
        private WrapPanel panelEventos;
        private string usuario;
        private TextBlock lblWelcome;
        private TextBox txtBuscar;

        public Inicio(string usuario)
        {
            this.usuario = usuario;
            this.Title = "EventPass MX";
            this.Width = 1100;
            this.Height = 650;
            this.WindowStyle = WindowStyle.None;
            this.WindowState = WindowState.Maximized;
            this.Background = new SolidColorBrush(Color.FromRgb(18, 18, 30));

            var root = new DockPanel();

            #region SIDEBAR
            var sidebar = new Border
            {
                Width = 60,
                Background = new SolidColorBrush(Color.FromRgb(11, 26, 38))
            };

            var sideStack = new StackPanel { Margin = new Thickness(5) };

            Button CrearBoton(string icono, string texto, RoutedEventHandler click)
            {
                var btn = new Button
                {
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    Foreground = Brushes.White,
                    Margin = new Thickness(5)
                };

                var sp = new StackPanel { Orientation = Orientation.Horizontal };
                sp.Children.Add(new TextBlock { Text = icono, FontSize = 16 });

                var lbl = new TextBlock
                {
                    Text = texto,
                    Margin = new Thickness(10, 0, 0, 0),
                    Opacity = 0
                };

                sp.Children.Add(lbl);
                btn.Content = sp;
                btn.Tag = lbl;
                btn.Click += click;

                return btn;
            }

            sideStack.Children.Add(CrearBoton("💱", "Reventa", (s, e) => new ResaleWindow(usuario).ShowDialog()));
            sideStack.Children.Add(CrearBoton("🧠", "Memoria", (s, e) => new MemoryWindow(usuario).ShowDialog()));
            sideStack.Children.Add(CrearBoton("📷", "Cámaras", (s, e) => new LiveCamerasWindow(usuario).ShowDialog()));
            sideStack.Children.Add(CrearBoton("🗳️", "Votar", (s, e) => new VotingWindow().ShowDialog()));
            sideStack.Children.Add(CrearBoton("🎁", "Rewards", (s, e) => new RewardsWindow(usuario).ShowDialog()));
            sideStack.Children.Add(CrearBoton("📜", "Historial", (s, e) => new HistorialWindow(usuario).ShowDialog()));

            sidebar.Child = sideStack;

            sidebar.MouseEnter += (s, e) =>
            {
                sidebar.Width = 200;
                foreach (Button b in sideStack.Children)
                    ((TextBlock)b.Tag).Opacity = 1;
            };

            sidebar.MouseLeave += (s, e) =>
            {
                sidebar.Width = 60;
                foreach (Button b in sideStack.Children)
                    ((TextBlock)b.Tag).Opacity = 0;
            };

            DockPanel.SetDock(sidebar, Dock.Left);
            root.Children.Add(sidebar);
            #endregion

            #region HEADER
            var header = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10)
            };

            lblWelcome = new TextBlock
            {
                Text = $"👋 {usuario} | Puntos: {DataStore.GetPoints(usuario)}",
                Foreground = Brushes.White,
                FontSize = 14
            };

            txtBuscar = new TextBox
            {
                Width = 200,
                Margin = new Thickness(20, 0, 0, 0),
                Text = "Buscar..."
            };

            txtBuscar.TextChanged += (s, e) => Filtrar();

            var btnSalir = new Button
            {
                Content = "Salir",
                Margin = new Thickness(20, 0, 0, 0)
            };

            btnSalir.Click += (s, e) =>
            {
                this.Close();
                Application.Current.Shutdown();
            };

            header.Children.Add(lblWelcome);
            header.Children.Add(txtBuscar);
            header.Children.Add(btnSalir);

            DockPanel.SetDock(header, Dock.Top);
            root.Children.Add(header);
            #endregion

            #region CONTENIDO
            var scroll = new ScrollViewer();
            panelEventos = new WrapPanel { Margin = new Thickness(10) };

            scroll.Content = panelEventos;
            root.Children.Add(scroll);
            #endregion

            this.Content = root;

            CargarEventos(DataStore.Eventos);
        }

        #region TARJETAS
        private Border CrearCard(Evento ev)
        {
            var card = new Border
            {
                Width = 250,
                Height = 220,
                Background = new SolidColorBrush(Color.FromRgb(35, 35, 60)),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(10)
            };

            var stack = new StackPanel { Margin = new Thickness(10) };

            stack.Children.Add(new TextBlock
            {
                Text = ev.Nombre,
                Foreground = Brushes.White,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            });

            stack.Children.Add(new TextBlock
            {
                Text = $"💰 ${ev.Precio}",
                Foreground = Brushes.LightGray
            });

            if (ev.Artista != null)
            {
                stack.Children.Add(new TextBlock
                {
                    Text = $"🎤 {ev.Artista.Nombre}",
                    Foreground = Brushes.Gray
                });
            }

            var btnGeneral = new Button { Content = "General" };
            var btnVIP = new Button { Content = "VIP" };
            var btnBack = new Button { Content = "Backstage" };

            btnGeneral.Click += (s, e) => Comprar(ev, AccessLevel.General);
            btnVIP.Click += (s, e) => Comprar(ev, AccessLevel.VIP);
            btnBack.Click += (s, e) => Comprar(ev, AccessLevel.Backstage);

            stack.Children.Add(btnGeneral);
            stack.Children.Add(btnVIP);
            stack.Children.Add(btnBack);

            card.Child = stack;

            card.MouseEnter += (s, e) =>
                card.Background = new SolidColorBrush(Color.FromRgb(60, 60, 100));

            card.MouseLeave += (s, e) =>
                card.Background = new SolidColorBrush(Color.FromRgb(35, 35, 60));

            return card;
        }
        #endregion

        #region COMPRA REAL
        private void Comprar(Evento ev, AccessLevel tipo)
        {
            var fila = new FilaWindow(usuario, ev);
            fila.Owner = this;
            fila.ShowDialog();

            var pago = new PaymentWindow(usuario, ev, tipo);
            pago.Owner = this;
            pago.ShowDialog();

            lblWelcome.Text = $"👋 {usuario} | Puntos: {DataStore.GetPoints(usuario)}";
        }
        #endregion

        #region FILTRO
        private void Filtrar()
        {
            var busqueda = txtBuscar.Text?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(busqueda) || busqueda == "Buscar...")
            {
                CargarEventos(DataStore.Eventos);
                return;
            }

            var lista = DataStore.Eventos.Where(e => 
                e.Nombre.ToLower().Contains(busqueda.ToLower()) ||
                e.Ciudad.ToLower().Contains(busqueda.ToLower()) ||
                e.Categoria.ToLower().Contains(busqueda.ToLower()) ||
                (e.Artista?.Nombre?.ToLower().Contains(busqueda.ToLower()) ?? false)
            ).ToList();

            if (lista.Count == 0)
            {
                panelEventos.Children.Clear();
                panelEventos.Children.Add(new TextBlock
                {
                    Text = "No se encontraron eventos.",
                    Foreground = Brushes.Gray,
                    FontSize = 14,
                    Margin = new Thickness(20)
                });
            }
            else
            {
                CargarEventos(lista);
            }
        }
        #endregion

        #region CARGA
        private void CargarEventos(List<Evento> eventos)
        {
            panelEventos.Children.Clear();

            foreach (var ev in eventos)
            {
                panelEventos.Children.Add(CrearCard(ev));
            }
        }
        #endregion
    }
}