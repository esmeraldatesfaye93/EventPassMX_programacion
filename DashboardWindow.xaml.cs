using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace EventPassMX_programacion
{
    public class DashboardWindow : Window
    {
        private string usuario;
        private ListBox listaEventos;
        private TextBlock txtDetails;

        public DashboardWindow(string user)
        {
            this.usuario = user;
            this.Title = "Inicio - EventPassMX";
            this.Width = 900;
            this.Height = 600;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var root = new DockPanel { LastChildFill = true, Margin = new Thickness(8) };

            // Top bar with welcome, history, features and logout
            var top = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(8) };
            top.Children.Add(new TextBlock { Text = $"Bienvenido, {usuario}", FontSize = 16, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0,0,12,0) });
            var btnHistory = new Button { Content = "Historial", Width = 90, Margin = new Thickness(0,0,8,0) };
            btnHistory.Click += BtnHistory_Click;
            top.Children.Add(btnHistory);
            var btnFeatures = new Button { Content = "Funciones", Width = 90, Margin = new Thickness(0,0,8,0) };
            btnFeatures.Click += BtnOpenFeatures_Click;
            top.Children.Add(btnFeatures);
            var btnLogout = new Button { Content = "Cerrar sesión", Width = 110 };
            btnLogout.Click += BtnLogout_Click;
            top.Children.Add(btnLogout);

            DockPanel.SetDock(top, Dock.Top);
            root.Children.Add(top);

            // Main area: left events list, right details + actions
            var main = new Grid { Margin = new Thickness(8) };
            main.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            main.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(320) });

            listaEventos = new ListBox();
            listaEventos.DisplayMemberPath = "Nombre";
            listaEventos.SelectionChanged += ListaEventos_SelectionChanged;
            Grid.SetColumn(listaEventos, 0);
            main.Children.Add(listaEventos);

            var panelRight = new StackPanel { Margin = new Thickness(8) };
            panelRight.Children.Add(new TextBlock { Text = "Detalles del evento", FontWeight = FontWeights.Bold });
            txtDetails = new TextBlock { TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0,8,0,8) };
            panelRight.Children.Add(txtDetails);

            var btnJoin = new Button { Content = "Unirse a la fila / Comprar", Margin = new Thickness(0,4,0,4) };
            btnJoin.Click += BtnJoin_Click;
            panelRight.Children.Add(btnJoin);

            Grid.SetColumn(panelRight, 1);
            main.Children.Add(panelRight);

            root.Children.Add(main);

            this.Content = root;

            LoadEvents();
        }

        private void BtnBoletos_Click(object sender, RoutedEventArgs e)
        {
            var w = new Inicio(usuario) { Owner = this };
            w.ShowDialog();
        }

        private void BtnReventa_Click(object sender, RoutedEventArgs e)
        {
            var w = new ResaleWindow(usuario) { Owner = this };
            w.ShowDialog();
        }

        private void BtnVIP_Click(object sender, RoutedEventArgs e)
        {
            var w = new VIPWindow(usuario) { Owner = this };
            w.ShowDialog();
        }

        private void BtnRewards_Click(object sender, RoutedEventArgs e)
        {
            var w = new RewardsWindow(usuario) { Owner = this };
            w.ShowDialog();
        }

        private void BtnVoting_Click(object sender, RoutedEventArgs e)
        {
            var w = new VotingWindow() { Owner = this };
            w.ShowDialog();
        }

        private void BtnMemory_Click(object sender, RoutedEventArgs e)
        {
            var w = new MemoryWindow(usuario) { Owner = this };
            w.ShowDialog();
        }

        // --- new helper methods to populate and handle events in the Dashboard main UI ---
        private void LoadEvents()
        {
            listaEventos.Items.Clear();
            foreach (var e in DataStore.Eventos)
            {
                listaEventos.Items.Add(e);
            }
        }

        private void ListaEventos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listaEventos.SelectedItem is Evento ev)
            {
                txtDetails.Text = $"Nombre: {ev.Nombre}\nPrecio: ${ev.Precio}\n(Fecha simulada)";
            }
            else
            {
                txtDetails.Text = string.Empty;
            }
        }

        private void BtnJoin_Click(object sender, RoutedEventArgs e)
        {
            var sel = listaEventos.SelectedItem as Evento;
            if (sel == null)
            {
                MessageBox.Show("Seleccione un evento.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var fila = new FilaWindow(usuario, sel);
            fila.Owner = this;
            fila.ShowDialog();
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            var tickets = DataStore.GetTicketsForUser(usuario);
            var win = new TicketsWindow(usuario, tickets) { Owner = this };
            win.ShowDialog();
        }

        private void BtnOpenFeatures_Click(object sender, RoutedEventArgs e)
        {
            // Open the dashboard features window (this window already contains buttons in previous version)
            // Open legacy Dashboard helper (no-op) or show message
            MessageBox.Show("Abrir panel de funciones completo.", "Funciones", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var login = this.Owner as Window;
            if (login != null)
            {
                this.Close();
                login.Show();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
    }
}
