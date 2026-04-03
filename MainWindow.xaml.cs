using System.Windows;
using System.Windows.Controls;

namespace EventPassMX_programacion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Handlers compatible with previous UI
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // read from controls (support both naming schemes)
            var user = (this.FindName("txtUser") as TextBox)?.Text?.Trim() ?? (this.FindName("UsernameTextBox") as TextBox)?.Text?.Trim();
            var pass = (this.FindName("txtPass") as PasswordBox)?.Password ?? (this.FindName("PasswordBox") as PasswordBox)?.Password;
            PerformLogin(user, pass);
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            OpenCreateWindow();
        }

        // New handlers matching provided XAML names
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var user = (this.FindName("txtUser") as TextBox)?.Text?.Trim() ?? (this.FindName("UsernameTextBox") as TextBox)?.Text?.Trim();
            var pass = (this.FindName("txtPass") as PasswordBox)?.Password ?? (this.FindName("PasswordBox") as PasswordBox)?.Password;
            PerformLogin(user, pass);
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            OpenCreateWindow();
        }

        private void PerformLogin(string user, string pass)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DataStore.ValidateUser(user, pass))
            {
                // clear fields if present
                if (this.FindName("UsernameTextBox") is TextBox ut) ut.Clear();
                if (this.FindName("PasswordBox") is PasswordBox pt) pt.Clear();
                if (this.FindName("txtUser") is TextBox ut2) ut2.Clear();
                if (this.FindName("txtPass") is PasswordBox pt2) pt2.Clear();

                var inicio = new Inicio(user);
                inicio.Owner = this;
                inicio.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenCreateWindow()
        {
            var win = new DetailedCreateAccountWindow();
            win.Owner = this;
            win.ShowDialog();
        }
    }
}
