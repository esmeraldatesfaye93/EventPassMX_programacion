using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EventPassMX_programacion
{
    public class MemoryWindow : Window
    {
        private string usuario;
        private ComboBox cbTickets;

        public MemoryWindow(string usuario)
        {
            this.usuario = usuario;
            this.Title = "Recuerdo digital";
            this.Width = 500;
            this.Height = 350;

            // 🎨 COLORES MEJORADOS (más claros)
            this.Background = new SolidColorBrush(Color.FromRgb(30, 30, 50)); // antes 18,18,30
            Brush primary = new SolidColorBrush(Color.FromRgb(55, 55, 80));   // más claro
            Brush accent = new SolidColorBrush(Color.FromRgb(140, 100, 255)); // más brillante
            Brush textColor = new SolidColorBrush(Color.FromRgb(230, 230, 255)); // no blanco puro

            var grid = new Grid { Margin = new Thickness(15) };

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // 🔹 TÍTULO
            var title = new TextBlock
            {
                Text = "Generar recuerdo",
                Foreground = textColor,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            };

            // 🔹 COMBO
            cbTickets = new ComboBox
            {
                Background = primary,
                Foreground = textColor,
                BorderBrush = accent,
                Height = 35
            };

            // 🔹 BOTÓN
            var btn = new Button
            {
                Content = "Generar",
                Background = accent,
                Foreground = Brushes.White,
                Height = 38,
                Margin = new Thickness(0, 10, 0, 0)
            };
            btn.Click += BtnGen_Click;

            Grid.SetRow(title, 0);
            Grid.SetRow(cbTickets, 1);
            Grid.SetRow(btn, 2);

            grid.Children.Add(title);
            grid.Children.Add(cbTickets);
            grid.Children.Add(btn);

            this.Content = grid;

            LoadTickets();
        }

        private void LoadTickets()
        {
            cbTickets.Items.Clear();
            var tickets = DataStore.GetTicketsForUser(usuario);

            foreach (var t in tickets)
            {
                cbTickets.Items.Add(new ComboBoxItem
                {
                    Content = $"{t.Evento.Nombre} - {t.FechaCompra}",
                    Tag = t.Id,
                    Foreground = Brushes.Black // 👈 importante para que se lea dentro del dropdown
                });
            }
        }

        private void BtnGen_Click(object sender, RoutedEventArgs e)
        {
            if (cbTickets.SelectedItem is ComboBoxItem it && it.Tag is Guid id)
            {
                var mem = DataStore.GenerateDigitalMemory(usuario, id);

                if (mem != null)
                {
                    MessageBox.Show(
                        $"Recuerdo generado:\nResumen: {mem.Summary}\nItems: {mem.Items.Count}",
                        "Ok",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No se pudo generar el recuerdo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione un ticket.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}