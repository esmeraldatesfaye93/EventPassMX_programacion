using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EventPassMX_programacion
{
    public class Inicio : Window
    {
        private WrapPanel panelEventos;
        private WrapPanel panelPopulares;
        private string usuario;
        private TextBlock lblWelcome;
        private TextBox txtBuscar;
        private ComboBox cbFecha;
        private ComboBox cbCiudad;
        private ComboBox cbCategoria;

        public Inicio(string usuario)
        {
            this.usuario = usuario;
            this.Title = "EventPass MX";
            this.Width = 1100;
            this.Height = 650;
            this.Background = new SolidColorBrush(Color.FromRgb(18, 18, 30));

            var root = new DockPanel();

            
            var sidebar = new Border
            {
                Width = 56,
                Background = new SolidColorBrush(Color.FromRgb(11, 26, 38)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(36, 71, 90)),
                BorderThickness = new Thickness(0,0,1,0)
            };

            var sideStack = new StackPanel { VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(6,12,6,12) };

            
            var ham = new StackPanel { Margin = new Thickness(6), Height = 28 };
            ham.Children.Add(new Rectangle { Height = 3, Fill = Brushes.White, RadiusX = 2, RadiusY = 2, Margin = new Thickness(0,2,0,2) });
            ham.Children.Add(new Rectangle { Height = 3, Fill = Brushes.White, RadiusX = 2, RadiusY = 2, Margin = new Thickness(0,2,0,2) });
            ham.Children.Add(new Rectangle { Height = 3, Fill = Brushes.White, RadiusX = 2, RadiusY = 2, Margin = new Thickness(0,2,0,2) });
            sideStack.Children.Add(ham);

            
            Button makeButton(string icon, string text, RoutedEventHandler handler)
            {
                var b = new Button { Background = Brushes.Transparent, BorderBrush = Brushes.Transparent, Foreground = Brushes.White, Padding = new Thickness(6), HorizontalContentAlignment = HorizontalAlignment.Left };
                var sp = new StackPanel { Orientation = Orientation.Horizontal };
                sp.Children.Add(new TextBlock { Text = icon, FontSize = 16, Margin = new Thickness(0,0,8,0) });
                var lbl = new TextBlock { Text = text, VerticalAlignment = VerticalAlignment.Center, Opacity = 0, Margin = new Thickness(4,0,0,0) };
                sp.Children.Add(lbl);
                b.Content = sp;
                b.Click += handler;
                
                b.Tag = lbl;
                return b;
            }

            var btnResale = makeButton("💱", "Reventa segura", (s, e) => { var w = new ResaleWindow(usuario); w.Owner = this; w.ShowDialog(); });
            var btnMemory = makeButton("🧠", "Memory", (s, e) => { var w = new MemoryWindow(usuario); w.Owner = this; w.ShowDialog(); });
            var btnCamera = makeButton("📷", "Cámara en vivo", (s, e) => { var w = new LiveCamerasWindow(usuario); w.Owner = this; w.ShowDialog(); });
            var btnVoting = makeButton("🗳️", "Votación", (s, e) => { var w = new VotingWindow(); w.Owner = this; w.ShowDialog(); });
            var btnRewards = makeButton("🎁", "Rewards", (s, e) => { var w = new RewardsWindow(usuario); w.Owner = this; w.ShowDialog(); });

            sideStack.Children.Add(btnResale);
            sideStack.Children.Add(btnMemory);
            sideStack.Children.Add(btnCamera);
            sideStack.Children.Add(btnVoting);
            sideStack.Children.Add(btnRewards);

            sidebar.Child = sideStack;
            DockPanel.SetDock(sidebar, Dock.Left);
            root.Children.Add(sidebar);

            void ExpandSidebar()
            {
                var da = new System.Windows.Media.Animation.DoubleAnimation(sidebar.Width, 200, new Duration(System.TimeSpan.FromMilliseconds(220)));
                sidebar.BeginAnimation(FrameworkElement.WidthProperty, da);
                foreach (var child in sideStack.Children)
                {
                    if (child is Button b && b.Tag is TextBlock lbl)
                    {
                        var opa = new System.Windows.Media.Animation.DoubleAnimation(0, 1, new Duration(System.TimeSpan.FromMilliseconds(220)));
                        lbl.BeginAnimation(UIElement.OpacityProperty, opa);
                    }
                }
            }

            void CollapseSidebar()
            {
                var da = new System.Windows.Media.Animation.DoubleAnimation(sidebar.Width, 56, new Duration(System.TimeSpan.FromMilliseconds(200)));
                sidebar.BeginAnimation(FrameworkElement.WidthProperty, da);
                foreach (var child in sideStack.Children)
                {
                    if (child is Button b && b.Tag is TextBlock lbl)
                    {
                        var opa = new System.Windows.Media.Animation.DoubleAnimation(1, 0, new Duration(System.TimeSpan.FromMilliseconds(180)));
                        lbl.BeginAnimation(UIElement.OpacityProperty, opa);
                    }
                }
            }

            sidebar.MouseEnter += (s, e) => ExpandSidebar();
            sidebar.MouseLeave += (s, e) => CollapseSidebar();

            
            var header = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

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

            var btnSalir = new Button { Content = "Salir", Margin = new Thickness(20, 0, 0, 0) };
            btnSalir.Click += (s, e) => { this.Close(); Application.Current.Shutdown(); };

            header.Children.Add(lblWelcome);
            header.Children.Add(txtBuscar);
            header.Children.Add(btnSalir);

            DockPanel.SetDock(header, Dock.Top);
            root.Children.Add(header);

            
            var contentDock = new DockPanel();

            var scroll = new ScrollViewer();
            panelEventos = new WrapPanel { Margin = new Thickness(10) };

            scroll.Content = panelEventos;
            contentDock.Children.Add(scroll);
            DockPanel.SetDock(contentDock, Dock.Top);
            root.Children.Add(contentDock);

            this.Content = root;

            // Set banner text to the nearest upcoming featured event
            var featured = DataStore.Eventos.OrderBy(e => e.Fecha).FirstOrDefault();
            if (featured != null)
            {
                // update banner text
                var banner = root.Children.OfType<Border>().FirstOrDefault();
                if (banner != null && banner.Child is TextBlock tb) tb.Text = $"Evento destacado: {featured.Nombre} - {featured.Fecha.ToShortDateString()}";
            }

            CargarEventos(DataStore.Eventos);
            CargarPopulares();
        }

       
        private Border CrearCard(Evento ev)
        {
            var card = new Border
            {
                Width = 250,
                Height = 200,
                Background = new SolidColorBrush(Color.FromRgb(35, 35, 60)),
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(10)
            };

            var stack = new StackPanel { Margin = new Thickness(10) };

            var nombre = new TextBlock
            {
                Text = ev.Nombre,
                Foreground = Brushes.White,
                FontSize = 16
            };

            // Labels
            if (ev.IsSoldOut)
            {
                var sold = new TextBlock { Text = "SOLD OUT", Foreground = Brushes.OrangeRed, FontWeight = FontWeights.Bold, Margin = new Thickness(0,4,0,0) };
                stack.Children.Add(sold);
            }
            else if (ev.IsNew)
            {
                var neu = new TextBlock { Text = "NUEVO", Foreground = Brushes.LightGreen, FontWeight = FontWeights.Bold, Margin = new Thickness(0,4,0,0) };
                stack.Children.Add(neu);
            }

            var precio = new TextBlock
            {
                Text = $"💰 ${ev.Precio}",
                Foreground = Brushes.LightGray
            };

            var artista = new TextBlock
            {
                Text = ev.Artista != null ? $"🎤 {ev.Artista.Nombre}" : "",
                Foreground = Brushes.Gray
            };

            var btnGeneral = new Button { Content = "General", Margin = new Thickness(0, 5, 0, 0) };
            var btnVIP = new Button { Content = "VIP 🔥", Margin = new Thickness(0, 5, 0, 0) };
            var btnBack = new Button { Content = "Backstage 🎯", Margin = new Thickness(0, 5, 0, 0) };

            btnGeneral.Click += (s, e) => Comprar(ev, AccessLevel.General);
            btnVIP.Click += (s, e) => Comprar(ev, AccessLevel.VIP);
            btnBack.Click += (s, e) => Comprar(ev, AccessLevel.Backstage);

            stack.Children.Add(nombre);
            stack.Children.Add(precio);
            stack.Children.Add(artista);
            stack.Children.Add(btnGeneral);
            stack.Children.Add(btnVIP);
            stack.Children.Add(btnBack);

            card.Child = stack;

            
            card.MouseEnter += (s, e) =>
            {
                card.Background = new SolidColorBrush(Color.FromRgb(60, 60, 100));
            };

            card.MouseLeave += (s, e) =>
            {
                card.Background = new SolidColorBrush(Color.FromRgb(35, 35, 60));
            };

            return card;
        }

        
        private void Comprar(Evento ev, AccessLevel tipo)
        {
            var fila = new FilaWindow(usuario, ev);
            fila.Owner = this;
            fila.ShowDialog();

            Ticket ticket = null;
            
            if (tipo == AccessLevel.VIP || tipo == AccessLevel.Backstage)
            {
                
                try
                {
                    var pkg = new DataStore.VIPPackage { Name = tipo.ToString(), Price = ev.Precio * 0.3m, Description = "Paquete VIP" };
                    ticket = DataStore.PurchaseVIP(usuario, ev, pkg);
                }
                catch
                {
                    
                    ticket = DataStore.CreateTicket(usuario, ev);
                    if (ticket != null) ticket.Access = tipo;
                }
            }
            else
            {
                ticket = DataStore.CreateTicket(usuario, ev);
            }

            if (ticket != null)
            {
                MessageBox.Show($"✅ Compra exitosa\nTipo: {tipo}\nEvento: {ev.Nombre}", "Compra", MessageBoxButton.OK, MessageBoxImage.Information);
                lblWelcome.Text = $"👋 {usuario} | Puntos: {DataStore.GetPoints(usuario)}";
            }
            else
            {
                MessageBox.Show("❌ Error en pago", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
        private void Filtrar()
        {
            var lista = DataStore.Eventos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtBuscar.Text) && txtBuscar.Text != "Buscar...")
            {
                lista = lista.Where(e => e.Nombre.ToLower().Contains(txtBuscar.Text.ToLower()));
            }

            // apply filters if present
            if (cbFecha != null && cbFecha.SelectedItem is ComboBoxItem fi)
            {
                var tag = fi.Tag as string;
                if (tag == "2w") lista = lista.Where(e => (e.Fecha - System.DateTime.Now).TotalDays <= 14 && (e.Fecha - System.DateTime.Now).TotalDays >= 0);
                else if (tag == "2m") lista = lista.Where(e => (e.Fecha - System.DateTime.Now).TotalDays <= 60 && (e.Fecha - System.DateTime.Now).TotalDays >= 0);
            }

            if (cbCiudad != null && cbCiudad.SelectedItem is ComboBoxItem ci && (ci.Tag as string) != "all")
            {
                var city = (ci.Tag as string) ?? ci.Content.ToString();
                lista = lista.Where(e => string.Equals(e.Ciudad, city, System.StringComparison.OrdinalIgnoreCase));
            }

            if (cbCategoria != null && cbCategoria.SelectedItem is ComboBoxItem ca && (ca.Tag as string) != "all")
            {
                var cat = (ca.Tag as string) ?? ca.Content.ToString();
                lista = lista.Where(e => string.Equals(e.Categoria, cat, System.StringComparison.OrdinalIgnoreCase));
            }

            CargarEventos(lista.ToList());
        }

        
        private void CargarEventos(List<Evento> eventos)
        {
            panelEventos.Children.Clear();

            foreach (var ev in eventos)
            {
                panelEventos.Children.Add(CrearCard(ev));
            }
        }

        private void CargarPopulares()
        {
            if (panelPopulares == null) return;
            panelPopulares.Children.Clear();
            // pick top 3 upcoming events by date not sold out
            var populares = DataStore.Eventos.Where(e => !e.IsSoldOut).OrderBy(e => e.Fecha).Take(3);
            foreach (var ev in populares)
            {
                var b = new Border { Width = 320, Height = 100, Background = new SolidColorBrush(Color.FromRgb(30,30,45)), CornerRadius = new CornerRadius(8), Margin = new Thickness(6) };
                var st = new StackPanel { Margin = new Thickness(8) };
                st.Children.Add(new TextBlock { Text = ev.Nombre, Foreground = Brushes.White, FontSize = 14, FontWeight = FontWeights.Bold });
                st.Children.Add(new TextBlock { Text = $"{ev.Ciudad} • {ev.Fecha.ToShortDateString()} • ${ev.Precio}", Foreground = Brushes.LightGray });
                b.Child = st;
                panelPopulares.Children.Add(b);
            }
        }
    }
}