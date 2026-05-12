using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;

namespace EventPassMX_programacion
{
    public class OrderConfirmationWindow : Window
    {
        private Ticket ticket;
        private string pdfPath;
        private int cantidad;
        private string asientos;

        public OrderConfirmationWindow(Ticket t, string path, int cant = 1, string seats = "N/A")
        {
            ticket = t;
            pdfPath = path;
            cantidad = cant;
            asientos = seats;

            this.Title = "Confirmación de Orden";
            this.Width = 600;
            this.Height = 700;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Background = new SolidColorBrush(Color.FromRgb(20, 20, 35));

            var scroll = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            var mainStack = new StackPanel { Margin = new Thickness(20) };

            
            mainStack.Children.Add(new TextBlock
            {
                Text = "✓ ¡PAGO EXITOSO!",
                Foreground = Brushes.LimeGreen,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            });

            
            mainStack.Children.Add(new Rectangle
            {
                Height = 2,
                Fill = Brushes.LimeGreen,
                Margin = new Thickness(0, 0, 0, 20)
            });

            
            mainStack.Children.Add(new TextBlock
            {
                Text = "INFORMACIÓN DEL EVENTO",
                Foreground = Brushes.White,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            mainStack.Children.Add(CreateInfoRow("Evento:", ticket.Evento.Nombre));
            mainStack.Children.Add(CreateInfoRow("Artista:", ticket.Evento.Artista?.Nombre ?? "N/A"));
            mainStack.Children.Add(CreateInfoRow("Fecha:", ticket.Evento.Fecha.ToString("dd/MM/yyyy HH:mm")));
            mainStack.Children.Add(CreateInfoRow("Lugar:", ticket.Evento.Ciudad));
            mainStack.Children.Add(CreateInfoRow("Categoría:", ticket.Evento.Categoria));

            mainStack.Children.Add(new TextBlock { Margin = new Thickness(0, 15, 0, 0) });

            
            mainStack.Children.Add(new TextBlock
            {
                Text = "INFORMACIÓN DE COMPRA",
                Foreground = Brushes.White,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            mainStack.Children.Add(CreateInfoRow("Comprador:", ticket.Usuario));
            mainStack.Children.Add(CreateInfoRow("Tipo de Acceso:", ticket.Access.ToString()));
            mainStack.Children.Add(CreateInfoRow("Cantidad de Boletos:", cantidad.ToString()));
            mainStack.Children.Add(CreateInfoRow("Asientos:", asientos));
            mainStack.Children.Add(CreateInfoRow("Fecha de Compra:", ticket.FechaCompra.ToString("dd/MM/yyyy HH:mm:ss")));

            mainStack.Children.Add(new TextBlock { Margin = new Thickness(0, 15, 0, 0) });

            
            mainStack.Children.Add(new TextBlock
            {
                Text = "INFORMACIÓN DE PAGO",
                Foreground = Brushes.White,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            });

            mainStack.Children.Add(CreateInfoRow("Precio Unitario:", $"${ticket.Precio}"));
            mainStack.Children.Add(new TextBlock
            {
                Text = $"TOTAL PAGADO: ${ticket.Precio * cantidad}",
                Foreground = Brushes.LimeGreen,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 10, 0, 20)
            });

            mainStack.Children.Add(CreateInfoRow("Folio:", ticket.QRCode, Brushes.Yellow));

            mainStack.Children.Add(new TextBlock { Margin = new Thickness(0, 20, 0, 0) });

            
            mainStack.Children.Add(new Rectangle
            {
                Height = 2,
                Fill = Brushes.DarkGray,
                Margin = new Thickness(0, 0, 0, 20)
            });

            
            var btnStack = new StackPanel { Orientation = Orientation.Vertical };

            var btnDescargar = new Button
            {
                Content = "📥 Descargar Boleto PDF",
                Height = 45,
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromRgb(45, 137, 239)),
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10),
                Cursor = System.Windows.Input.Cursors.Hand
            };
            btnDescargar.Click += BtnDescargar_Click;
            btnStack.Children.Add(btnDescargar);

            var btnVer = new Button
            {
                Content = "👁️ Ver Boleto PDF",
                Height = 45,
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromRgb(40, 100, 40)),
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10),
                Cursor = System.Windows.Input.Cursors.Hand
            };
            btnVer.Click += BtnVer_Click;
            btnStack.Children.Add(btnVer);

            var btnCerrar = new Button
            {
                Content = "✓ Cerrar",
                Height = 45,
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromRgb(150, 150, 150)),
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Cursor = System.Windows.Input.Cursors.Hand
            };
            btnCerrar.Click += (s, e) => this.Close();
            btnStack.Children.Add(btnCerrar);

            mainStack.Children.Add(btnStack);

            scroll.Content = mainStack;
            this.Content = scroll;
        }

        private TextBlock CreateInfoRow(string label, string value, SolidColorBrush valueBrush = null)
        {
            var row = new TextBlock { Margin = new Thickness(0, 5, 0, 5) };
            var labelRun = new Run { Text = $"{label} ", Foreground = Brushes.Gray };
            var valueRun = new Run
            {
                Text = value,
                Foreground = valueBrush ?? Brushes.White
            };
            row.Inlines.Add(labelRun);
            row.Inlines.Add(valueRun);
            return row;
        }

        private void BtnDescargar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string destPath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    System.IO.Path.GetFileName(pdfPath)
                );

                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Copy(pdfPath, destPath, true);
                    MessageBox.Show($"Boleto descargado exitosamente en:\n{destPath}", "Descarga Exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("El archivo PDF no se encontró.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnVer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (System.IO.File.Exists(pdfPath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = pdfPath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("El archivo PDF no se encontró.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir el PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
