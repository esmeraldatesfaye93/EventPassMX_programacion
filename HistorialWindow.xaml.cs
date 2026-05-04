using System.Windows;
using System.Collections.Generic;

namespace EventPassMX_programacion
{
    public partial class HistorialWindow : Window
    {
        public HistorialWindow(string usuario)
        {
            InitializeComponent();
            CargarDatos(usuario);
        }

        private void CargarDatos(string usuario)
        {
            var tickets = DataStore.GetTicketsForUser(usuario);

            var lista = new List<dynamic>();

            foreach (var t in tickets)
            {
                lista.Add(new
                {
                    Id = t.Id,
                    Evento = t.Evento.Nombre,
                    Fecha = t.FechaCompra,
                    Cantidad = t.Cantidad,
                    Total = t.Precio
                });
            }

            tablaHistorial.ItemsSource = lista;
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
