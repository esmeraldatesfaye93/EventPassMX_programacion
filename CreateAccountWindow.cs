using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace EventPassMX_programacion
{
    public class CreateAccountWindow : Window
    {
        private TextBox txtNewUser;
        private PasswordBox txtNewPass;

        public CreateAccountWindow()
        {
            this.Title = "Crear cuenta";
            this.Width = 480;
            this.Height = 360;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            
            Brush panelBg = Application.Current.Resources.Contains("PanelBackgroundBrush") ? (Brush)Application.Current.Resources["PanelBackgroundBrush"] : new SolidColorBrush(Color.FromRgb(11, 26, 38));
            Brush accent = Application.Current.Resources.Contains("AccentBrush") ? (Brush)Application.Current.Resources["AccentBrush"] : new SolidColorBrush(Color.FromRgb(45,137,239));

            
            this.Background = new LinearGradientBrush(Color.FromRgb(7,18,38), Color.FromRgb(15,36,51), 45);

            var rootBorder = new Border
            {
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(16),
                Background = panelBg,
                Width = 420,
                Height = 300,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                RenderTransform = new TranslateTransform(0, 8),
                Opacity = 0
            };

            
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            
            var left = new StackPanel { Margin = new Thickness(6) };
            left.Children.Add(new TextBlock { Text = "EventPass", Foreground = accent, FontSize = 20, FontWeight = FontWeights.Bold });
            left.Children.Add(new TextBlock { Text = "Crear tu cuenta", Foreground = Brushes.White, Margin = new Thickness(0,6,0,10) });
            left.Children.Add(new TextBlock { Text = "Únete y comienza a comprar entradas para los mejores eventos en México.", Foreground = new SolidColorBrush(Color.FromRgb(159,182,195)), TextWrapping = TextWrapping.Wrap });

            Grid.SetColumn(left, 0);
            grid.Children.Add(left);

            
            var right = new StackPanel { Margin = new Thickness(6) };
            right.Children.Add(new TextBlock { Text = "Usuario", Foreground = new SolidColorBrush(Color.FromRgb(159,182,195)), FontSize = 12 });
            txtNewUser = new TextBox { Margin = new Thickness(0,6,0,6), Padding = new Thickness(8), Foreground = Brushes.White };
            if (Application.Current.Resources.Contains("InputBox")) txtNewUser.Style = (Style)Application.Current.Resources["InputBox"];
            right.Children.Add(txtNewUser);

            right.Children.Add(new TextBlock { Text = "Contraseña", Foreground = new SolidColorBrush(Color.FromRgb(159,182,195)), FontSize = 12 });
            txtNewPass = new PasswordBox { Margin = new Thickness(0,6,0,6), Padding = new Thickness(8), Foreground = Brushes.White };
            if (Application.Current.Resources.Contains("InputBox")) txtNewPass.Style = (Style)Application.Current.Resources["InputBox"];
            right.Children.Add(txtNewPass);

            var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0,12,0,0) };
            var btnCancel = new Button { Content = "Cancelar", Width = 100, Margin = new Thickness(0,0,8,0) };
            if (Application.Current.Resources.Contains("SecondaryButton")) btnCancel.Style = (Style)Application.Current.Resources["SecondaryButton"];
            btnCancel.Click += (s, e) => this.Close();

            var btnCreate = new Button { Content = "Crear cuenta", Width = 140 };
            if (Application.Current.Resources.Contains("PrimaryButton")) btnCreate.Style = (Style)Application.Current.Resources["PrimaryButton"];
            btnCreate.Click += BtnCreate_Click;

            buttons.Children.Add(btnCancel);
            buttons.Children.Add(btnCreate);

            right.Children.Add(buttons);

            Grid.SetColumn(right, 1);
            grid.Children.Add(right);

            rootBorder.Child = grid;
            this.Content = rootBorder;

            
            this.Loaded += (s, e) =>
            {
                var da = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(360))) { EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut } };
                var ta = new DoubleAnimation(8, 0, new Duration(TimeSpan.FromMilliseconds(360))) { EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut } };
                rootBorder.BeginAnimation(UIElement.OpacityProperty, da);
                (rootBorder.RenderTransform as TranslateTransform).BeginAnimation(TranslateTransform.YProperty, ta);
            };
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var user = txtNewUser.Text?.Trim();
            var pass = txtNewPass.Password;

            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Por favor ingresa un usuario.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNewUser.Focus();
                return;
            }

            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Por favor ingresa una contraseña.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNewPass.Focus();
                return;
            }

            if (user.Length < 3)
            {
                MessageBox.Show("El usuario debe tener al menos 3 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNewUser.Focus();
                return;
            }

            if (pass.Length < 3)
            {
                MessageBox.Show("La contraseña debe tener al menos 3 caracteres.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNewPass.Focus();
                return;
            }

            if (DataStore.AddUser(user, pass))
            {
                MessageBox.Show($"¡Cuenta creada exitosamente! Bienvenido {user}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Este usuario ya existe. Por favor elige otro.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtNewUser.Focus();
                txtNewUser.SelectAll();
            }
        }

        private void AddHoverEffect(Button btn, Color hover, Color normal)
        {
            var hb = new SolidColorBrush(normal);
            btn.Background = hb;
            btn.MouseEnter += (s, e) => hb.Color = hover;
            btn.MouseLeave += (s, e) => hb.Color = normal;
        }
    }
}
