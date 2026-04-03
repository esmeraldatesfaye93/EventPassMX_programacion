using System;
using System.Windows;
using System.Windows.Controls;

namespace EventPassMX_programacion
{
    public class TicketWindow : Window
    {
        private Ticket ticket;

        public TicketWindow(Ticket ticket)
        {
            this.ticket = ticket;
            this.Title = "Ticket digital";
            this.Width = 420;
            this.Height = 260;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var stack = new StackPanel { Margin = new Thickness(12) };
            stack.Children.Add(new TextBlock { Text = "Ticket generado", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0,0,0,8) });

            stack.Children.Add(new TextBlock { Text = $"ID: {ticket.Id}" });
            stack.Children.Add(new TextBlock { Text = $"Usuario: {ticket.Usuario}" });
            stack.Children.Add(new TextBlock { Text = $"Evento: {ticket.Evento.Nombre}" });
            stack.Children.Add(new TextBlock { Text = $"Precio: ${ticket.Precio}" });
            stack.Children.Add(new TextBlock { Text = $"Fecha compra: {ticket.FechaCompra}" });

            var btnClose = new Button { Content = "Cerrar", Width = 80, Margin = new Thickness(0,12,0,0), HorizontalAlignment = HorizontalAlignment.Right };
            btnClose.Click += (s, e) => this.Close();
            stack.Children.Add(btnClose);

            this.Content = stack;
        }
    }
}
