using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace EventPassMX_programacion
{
    public class FilaWindow : Window
    {
        private string usuario;
        private Evento evento;
        private int posicion;
        private TextBlock txtStatus;
        private DispatcherTimer timer;

        public FilaWindow(string usuario, Evento evento)
        {
            this.usuario = usuario;
            this.evento = evento;
            this.Title = "Fila virtual";
            this.Width = 350;
            this.Height = 180;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var stack = new StackPanel { Margin = new Thickness(12) };
            stack.Children.Add(new TextBlock { Text = $"Evento: {evento.Nombre}", FontWeight = FontWeights.Bold });
            stack.Children.Add(new TextBlock { Text = $"Precio: ${evento.Precio}", Margin = new Thickness(0,4,0,8) });

            txtStatus = new TextBlock { Text = "Calculando posición...", Margin = new Thickness(0,8,0,8) };
            stack.Children.Add(txtStatus);

            var btnCancel = new Button { Content = "Cancelar", Width = 80, HorizontalAlignment = HorizontalAlignment.Right };
            btnCancel.Click += (s, e) => this.Close();
            stack.Children.Add(btnCancel);

            this.Content = stack;

            
            var rnd = new Random();
            posicion = rnd.Next(3, 10);
            txtStatus.Text = $"Tu posición en la fila: {posicion} \nTiempo estimado: {posicion * 5} seg";

            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            posicion--;
            if (posicion > 0)
            {
                txtStatus.Text = $"Tu posición en la fila: {posicion} \nTiempo estimado: {posicion * 5} seg";
            }
            else
            {
                timer.Stop();
                txtStatus.Text = "Es tu turno. Generando ticket...";
                var ticket = DataStore.CreateTicket(usuario, evento);
                if (ticket != null)
                {
                    var twin = new TicketWindow(ticket);
                    twin.Owner = this.Owner;
                    twin.ShowDialog();
                }
                MessageBox.Show("Compra completada.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }
    }
}
