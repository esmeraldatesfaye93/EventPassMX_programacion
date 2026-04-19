using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace EventPassMX_programacion
{
    public class HistorialWindow : Window
    {
        public HistorialWindow(string usuario)
        {
            this.Title = "Historial de compras";
            this.Width = 400;
            this.Height = 400;

            var lista = new ListBox();

            var tickets = DataStore.GetTicketsForUser(usuario);

            foreach (var t in tickets)
            {
                var btn = new Button
                {
                    Content = $"{t.Evento.Nombre} - ${t.Precio}"
                };

                btn.Click += (s, e) =>
                {
                    var ruta = DataStore.GenerarTicketArchivo(t);
                    MessageBox.Show($"Ticket guardado en:\n{ruta}");
                };

                lista.Items.Add(btn);
            }

            this.Content = lista;
        }
    }
}