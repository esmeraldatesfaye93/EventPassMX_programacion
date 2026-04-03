using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EventPassMX_programacion
{
    public class DetailedCreateAccountWindow : Window
    {
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtUsername;
        private PasswordBox txtPassword;

        public DetailedCreateAccountWindow()
        {
            this.Title = "Crear cuenta - Detallado";
            this.Width = 420;
            this.Height = 420;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var root = new StackPanel { Margin = new Thickness(12) };

            root.Children.Add(new TextBlock { Text = "Crear cuenta", FontSize = 16, FontWeight = FontWeights.Bold, Margin = new Thickness(0,0,0,8) });

            root.Children.Add(new TextBlock { Text = "Nombre" });
            txtFirstName = new TextBox { Margin = new Thickness(0,4,0,8) };
            root.Children.Add(txtFirstName);

            root.Children.Add(new TextBlock { Text = "Apellido" });
            txtLastName = new TextBox { Margin = new Thickness(0,4,0,8) };
            root.Children.Add(txtLastName);

            root.Children.Add(new TextBlock { Text = "Teléfono" });
            txtPhone = new TextBox { Margin = new Thickness(0,4,0,8) };
            root.Children.Add(txtPhone);

            root.Children.Add(new TextBlock { Text = "Correo" });
            txtEmail = new TextBox { Margin = new Thickness(0,4,0,8) };
            root.Children.Add(txtEmail);

            root.Children.Add(new TextBlock { Text = "Nombre de usuario" });
            txtUsername = new TextBox { Margin = new Thickness(0,4,0,8) };
            root.Children.Add(txtUsername);

            root.Children.Add(new TextBlock { Text = "Contraseña" });
            txtPassword = new PasswordBox { Margin = new Thickness(0,4,0,12) };
            root.Children.Add(txtPassword);

            var panelButtons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            var btnCreate = new Button { Content = "Crear", Width = 100, Margin = new Thickness(0,0,8,0) };
            var btnCancel = new Button { Content = "Cancelar", Width = 100 };
            btnCreate.Click += BtnCreate_Click;
            btnCancel.Click += (s, e) => this.Close();
            panelButtons.Children.Add(btnCreate);
            panelButtons.Children.Add(btnCancel);

            root.Children.Add(panelButtons);

            this.Content = root;
        }

        private async void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            var first = txtFirstName.Text?.Trim();
            var last = txtLastName.Text?.Trim();
            var phone = txtPhone.Text?.Trim();
            var email = txtEmail.Text?.Trim();
            var username = txtUsername.Text?.Trim();
            var password = txtPassword.Password ?? string.Empty;

            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(last) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Complete todos los campos requeridos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            if (DataStore.GetUser(username) != null)
            {
                MessageBox.Show("El nombre de usuario ya existe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newUser = new User
            {
                Username = username,
                Password = password,
                FirstName = first,
                LastName = last,
                Phone = phone,
                Email = email
            };

            var added = DataStore.AddUser(newUser);
            if (!added)
            {
                MessageBox.Show("Error al crear la cuenta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Cuenta creada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            
            var main = this.Owner as MainWindow;
            if (main != null)
            {
                var inicio = new Inicio(newUser.Username);
                inicio.Owner = main;
                inicio.Show();
                main.Hide();
            }

            this.Close();
        }

     
    }
}
