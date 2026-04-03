using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace EventPassMX_programacion
{
    
    public class Inicio : Window
    {
        private ListBox listaEventos;
        private string usuario;

        public Inicio(string usuario)
        {
            this.usuario = usuario;
            this.Title = "Eventos - EventPass MX";
            this.Width = 600;
            this.Height = 400;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var root = new DockPanel();

            var top = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(8) };
            var lbl = new TextBlock { Text = $"Bienvenido, {usuario}", FontSize = 14, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0,0,12,0) };
            var btnHistory = new Button { Content = "Historial", Width = 80, Margin = new Thickness(0,0,8,0) };
            btnHistory.Click += BtnHistory_Click;
            var btnLogout = new Button { Content = "Cerrar sesión", Width = 100 };
            btnLogout.Click += BtnLogout_Click;

            top.Children.Add(lbl);
            top.Children.Add(btnHistory);
            top.Children.Add(btnLogout);

            DockPanel.SetDock(top, Dock.Top);
            root.Children.Add(top);

            var main = new Grid { Margin = new Thickness(8) };
            main.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            main.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });

            listaEventos = new ListBox();
            foreach (var e in DataStore.Eventos)
            {
                listaEventos.Items.Add(e);
            }
            listaEventos.DisplayMemberPath = "Nombre";
            Grid.SetColumn(listaEventos, 0);
            main.Children.Add(listaEventos);

            var panelRight = new StackPanel { Margin = new Thickness(8) };
            var lblInfo = new TextBlock { Text = "Detalles del evento", FontWeight = FontWeights.Bold };
            panelRight.Children.Add(lblInfo);

            var txtDetails = new TextBlock { TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0,8,0,8) };
            panelRight.Children.Add(txtDetails);

            var btnJoin = new Button { Content = "Unirse a la fila / Comprar", Margin = new Thickness(0,4,0,4) };
            btnJoin.Click += (s, e) =>
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
            };
            panelRight.Children.Add(btnJoin);

            listaEventos.SelectionChanged += (s, e) =>
            {
                var sel = listaEventos.SelectedItem as Evento;
                if (sel != null)
                {
                    txtDetails.Text = $"Nombre: {sel.Nombre}\nPrecio: ${sel.Precio}\n(Fecha simulada)";
                }
                else
                {
                    txtDetails.Text = string.Empty;
                }
            };

            Grid.SetColumn(panelRight, 1);
            main.Children.Add(panelRight);

            root.Children.Add(main);

            this.Content = root;
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

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
        {
            var tickets = DataStore.GetTicketsForUser(usuario);
            var win = new TicketsWindow(usuario, tickets);
            win.Owner = this;
            win.ShowDialog();
        }
    }
}
