using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EventPassMX_programacion
{
    public class TicketsWindow : Window
    {
        public TicketsWindow(string usuario, List<Ticket> tickets)
        {
            this.Title = "Historial de tickets";
            this.Width = 500;
            this.Height = 350;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var stack = new StackPanel { Margin = new Thickness(12) };
            stack.Children.Add(new TextBlock { Text = $"Tickets de {usuario}", FontSize = 14, FontWeight = FontWeights.Bold, Margin = new Thickness(0,0,0,8) });

            var list = new ListBox();
            foreach (var t in tickets)
            {
                list.Items.Add($"{t.FechaCompra}: {t.Evento.Nombre} - ${t.Precio} (ID: {t.Id})");
            }

            stack.Children.Add(list);

            var btnClose = new Button { Content = "Cerrar", Width = 80, Margin = new Thickness(0,12,0,0), HorizontalAlignment = HorizontalAlignment.Right };
            btnClose.Click += (s, e) => this.Close();
            stack.Children.Add(btnClose);

            this.Content = stack;
        }
    }
}
