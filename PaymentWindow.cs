using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;

namespace EventPassMX_programacion
{
    public class PaymentWindow : Window
    {
        private string usuario;
        private Evento evento;
        private AccessLevel tipo;

        private TextBox txtCard;
        private TextBox txtName;
        private TextBox txtQuantity;

        public PaymentWindow(string user, Evento ev, AccessLevel t)
        {
            usuario = user;
            evento = ev;
            tipo = t;

            this.Title = "Pago Seguro";
            this.Width = 450;
            this.Height = 420;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Background = new SolidColorBrush(Color.FromRgb(20, 20, 35));

            var scroll = new ScrollViewer { VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            var stack = new StackPanel { Margin = new Thickness(20) };

            
            stack.Children.Add(new TextBlock
            {
                Text = "PAGO SEGURO",
                Foreground = Brushes.White,
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 15)
            });

            
            stack.Children.Add(new TextBlock { Text = "Evento", Foreground = Brushes.Gray, FontSize = 11 });
            stack.Children.Add(new TextBlock { Text = evento.Nombre, Foreground = Brushes.White, FontSize = 14, Margin = new Thickness(0, 0, 0, 10) });

            stack.Children.Add(new TextBlock { Text = "Tipo de Acceso", Foreground = Brushes.Gray, FontSize = 11 });
            stack.Children.Add(new TextBlock { Text = tipo.ToString(), Foreground = Brushes.LimeGreen, FontSize = 14, Margin = new Thickness(0, 0, 0, 10) });

            
            stack.Children.Add(new TextBlock { Text = "Cantidad de Boletos", Foreground = Brushes.Gray, FontSize = 11 });
            txtQuantity = new TextBox
            {
                Height = 32,
                Padding = new Thickness(10),
                Text = "1",
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromRgb(30, 30, 50)),
                CaretBrush = Brushes.White,
                Margin = new Thickness(0, 0, 0, 10)
            };
            stack.Children.Add(txtQuantity);

            
            var totalPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 15) };
            totalPanel.Children.Add(new TextBlock { Text = "Total: ", Foreground = Brushes.Gray });
            var totalBlock = new TextBlock
            {
                Text = $"${evento.Precio}",
                Foreground = Brushes.LimeGreen,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };
            totalPanel.Children.Add(totalBlock);
            stack.Children.Add(totalPanel);

            
            stack.Children.Add(new TextBlock { Text = "Número de Tarjeta", Foreground = Brushes.Gray, FontSize = 11 });
            txtCard = new TextBox
            {
                Height = 32,
                Padding = new Thickness(10),
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromRgb(30, 30, 50)),
                CaretBrush = Brushes.White,
                Margin = new Thickness(0, 0, 0, 10)
            };
            txtCard.TextChanged += (s, e) => UpdateTotal(totalBlock);
            stack.Children.Add(txtCard);

            
            stack.Children.Add(new TextBlock { Text = "Nombre en Tarjeta", Foreground = Brushes.Gray, FontSize = 11 });
            txtName = new TextBox
            {
                Height = 32,
                Padding = new Thickness(10),
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromRgb(30, 30, 50)),
                CaretBrush = Brushes.White,
                Margin = new Thickness(0, 0, 0, 15)
            };
            stack.Children.Add(txtName);

            
            var btnStack = new StackPanel { Orientation = Orientation.Horizontal };
            var btnPagar = new Button
            {
                Content = "Pagar Ahora",
                Background = Brushes.Green,
                Foreground = Brushes.White,
                Height = 45,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 10, 0),
                Cursor = System.Windows.Input.Cursors.Hand,
                Padding = new Thickness(15, 10, 15, 10)
            };
            btnPagar.Click += BtnPagar_Click;

            var btnCancelar = new Button
            {
                Content = "Cancelar",
                Background = new SolidColorBrush(Color.FromRgb(200, 50, 50)),
                Foreground = Brushes.White,
                Height = 45,
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Cursor = System.Windows.Input.Cursors.Hand,
                Padding = new Thickness(15, 10, 15, 10)
            };
            btnCancelar.Click += (s, e) => this.Close();

            btnStack.Children.Add(btnPagar);
            btnStack.Children.Add(btnCancelar);

            stack.Children.Add(btnStack);

            scroll.Content = stack;
            this.Content = scroll;
        }

        private void UpdateTotal(TextBlock totalBlock)
        {
            if (int.TryParse(txtQuantity.Text, out int cantidad) && cantidad > 0)
            {
                totalBlock.Text = $"${evento.Precio * cantidad}";
            }
        }

        private void BtnPagar_Click(object sender, RoutedEventArgs e)
        {
            
            if (!int.TryParse(txtQuantity.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Por favor ingresa una cantidad válida de boletos (mínimo 1).", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQuantity.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCard.Text))
            {
                MessageBox.Show("Por favor ingresa el número de tarjeta.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCard.Focus();
                return;
            }

            if (txtCard.Text.Length < 13)
            {
                MessageBox.Show("El número de tarjeta debe tener al menos 13 dígitos.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtCard.Focus();
                return;
            }


            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Por favor ingresa el nombre en la tarjeta.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            if (txtName.Text.Length < 3)
            {
                MessageBox.Show("El nombre en la tarjeta debe tener al menos 3 caracteres.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            try
            {
                Ticket ticket = null;

                if (tipo == AccessLevel.General)
                {
                    ticket = DataStore.CreateTicket(usuario, evento);
                }
                else
                {
                    var pkg = new DataStore.VIPPackage
                    {
                        Name = tipo.ToString(),
                        Price = evento.Precio * 0.3m,
                        Description = "VIP"
                    };

                    ticket = DataStore.PurchaseVIP(usuario, evento, pkg);

                    if (ticket != null)
                        ticket.Access = tipo;
                }

                if (ticket != null)
                {

                    string asientos = "";

                    if (evento.Nombre.Contains("Stairway To The Sky Tour"))
                    {
                        asientos = "VIP A1, VIP A2";
                    }
                    else if (evento.Nombre.Contains("After Hours Til Dawn"))
                    {
                        asientos = "B15, B16";
                    }
                    else if (evento.Nombre.Contains("Tecate Pa'l Norte 2026"))
                    {
                        asientos = "Zona General";
                    }
                    else
                    {
                        asientos = "N/A";
                    }


                    var path = GeneradorBoletos.GeneratePDF(ticket, cantidad, asientos);

                    var confirmWindow = new OrderConfirmationWindow(ticket, path, cantidad, asientos);
                    confirmWindow.Owner = this;
                    confirmWindow.ShowDialog();


                    new HistorialWindow(usuario).Show();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al procesar el pago. Intenta de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al procesar el pago:\n{ex.Message}\n\nDetalles: {ex.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}