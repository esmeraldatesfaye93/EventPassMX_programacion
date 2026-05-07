using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EventPassMX_programacion
{
    public class SimpleLoginWindow : Window
    {
        private TextBox txtUser;
        private PasswordBox txtPass;

        public SimpleLoginWindow()
        {
            this.Title = "Login - EventPass MX";
            this.Width = 300;
            this.Height = 315;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Background = new SolidColorBrush(Color.FromRgb(30, 30, 45));
            this.ResizeMode = ResizeMode.NoResize;

            var stack = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 220
            };

            // LOGO
            var logo = new Image
            {
                Source = new BitmapImage(new Uri(
                    @"C:\Users\EsmeraldaOviedo\Documents\EventPassMX_Programacion\LOGO EVENTPASSMX.jpg",
                    UriKind.Absolute)),

                Width = 90,
                Height = 90,
                Margin = new Thickness(0, 0, 0, 10),
                Stretch = Stretch.Uniform
            };

            stack.Children.Add(logo);

            // TÍTULO
            stack.Children.Add(new TextBlock
            {
                Text = "EventPass MX",
                Foreground = Brushes.White,
                FontSize = 20,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 15)
            });

            // USUARIO
            stack.Children.Add(new TextBlock
            {
                Text = "Usuario",
                Foreground = Brushes.LightGray,
                Margin = new Thickness(0, 0, 0, 3)
            });

            txtUser = new TextBox
            {
                Margin = new Thickness(0, 5, 0, 10),
                Width = 220,
                Height = 30,
                FontSize = 14
            };

            stack.Children.Add(txtUser);

            // CONTRASEÑA
            stack.Children.Add(new TextBlock
            {
                Text = "Contraseña",
                Foreground = Brushes.LightGray,
                Margin = new Thickness(0, 0, 0, 3)
            });

            txtPass = new PasswordBox
            {
                Margin = new Thickness(0, 5, 0, 10),
                Width = 220,
                Height = 30,
                FontSize = 14
            };

            stack.Children.Add(txtPass);

            // BOTONES
            var buttons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 15, 0, 0)
            };

            var btnLogin = new Button
            {
                Content = "Iniciar sesión",
                Width = 100,
                Height = 35,
                Margin = new Thickness(0, 0, 8, 0),
                Background = new SolidColorBrush(Color.FromRgb(45, 137, 239)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0)
            };

            btnLogin.Click += BtnLogin_Click;

            var btnCreate = new Button
            {
                Content = "Crear cuenta",
                Width = 100,
                Height = 35,
                Background = new SolidColorBrush(Color.FromRgb(16, 124, 16)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0)
            };

            btnCreate.Click += BtnCreate_Click;

            buttons.Children.Add(btnLogin);
            buttons.Children.Add(btnCreate);

            stack.Children.Add(buttons);

            this.Content = stack;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = txtUser.Text?.Trim();
            var pass = txtPass.Password;

            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show(
                    "Por favor ingresa tu usuario.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                txtUser.Focus();
                return;
            }

            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show(
                    "Por favor ingresa tu contraseña.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                txtPass.Focus();
                return;
            }

            if (user.Length < 3)
            {
                MessageBox.Show(
                    "El usuario debe tener al menos 3 caracteres.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                txtUser.Focus();
                return;
            }

            if (pass.Length < 3)
            {
                MessageBox.Show(
                    "La contraseña debe tener al menos 3 caracteres.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                txtPass.Focus();
                return;
            }

            if (DataStore.ValidateUser(user, pass))
            {
                MessageBox.Show(
                    $"¡Bienvenido {user}!",
                    "Éxito",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                var inicio = new Inicio(user);
                inicio.Owner = this;
                inicio.Show();

                this.Hide();
            }
            else
            {
                MessageBox.Show(
                    "Usuario o contraseña incorrectos.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                txtPass.Clear();
                txtPass.Focus();
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var win = new CreateAccountWindow();
            win.Owner = this;
            win.ShowDialog();
        }
    }
}