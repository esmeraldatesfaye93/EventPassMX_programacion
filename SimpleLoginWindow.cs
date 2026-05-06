using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EventPassMX_programacion
{
    
    public class SimpleLoginWindow : Window
    {
        private TextBox txtUser;
        private PasswordBox txtPass;

        public SimpleLoginWindow()
        {
            this.Title = "Login - EventPass MX";
            this.Width = 350;
            this.Height = 250;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Background = new SolidColorBrush(Color.FromRgb(30, 30, 45));

            var stack = new StackPanel { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, Width = 220 };

            stack.Children.Add(new TextBlock { Text = "EventPass MX", Foreground = Brushes.White, FontSize = 18, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0,0,0,10) });

            txtUser = new TextBox { Margin = new Thickness(0,5,0,5), Width = 220 };
            stack.Children.Add(txtUser);
            txtPass = new PasswordBox { Margin = new Thickness(0,5,0,5), Width = 220 };
            stack.Children.Add(txtPass);

            var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0,12,0,0) };
            var btnLogin = new Button { Content = "Iniciar sesión", Width = 100, Margin = new Thickness(0,0,8,0), Background = new SolidColorBrush(Color.FromRgb(45,137,239)), Foreground = Brushes.White };
            btnLogin.Click += BtnLogin_Click;
            var btnCreate = new Button { Content = "Crear cuenta", Width = 100, Background = new SolidColorBrush(Color.FromRgb(16,124,16)), Foreground = Brushes.White };
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
                MessageBox.Show("Por favor ingresa tu usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUser.Focus();
                return;
            }

            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Por favor ingresa tu contraseña.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPass.Focus();
                return;
            }

            if (user.Length < 3)
            {
                MessageBox.Show("El usuario debe tener al menos 3 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUser.Focus();
                return;
            }

            if (pass.Length < 3)
            {
                MessageBox.Show("La contraseña debe tener al menos 3 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPass.Focus();
                return;
            }

            if (DataStore.ValidateUser(user, pass))
            {
                MessageBox.Show($"¡Bienvenido {user}!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                var inicio = new Inicio(user);
                inicio.Owner = this;
                inicio.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
