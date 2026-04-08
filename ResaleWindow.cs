using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EventPassMX_programacion
{
    public class ResaleWindow : Window
    {
        private string usuario;
        private ListBox lstResales;
        private ComboBox cbMyTickets;
        private TextBox txtPrice;

        public ResaleWindow(string usuario)
        {
            this.usuario = usuario;
            this.Title = "Reventa segura";
            this.Width = 600;
            this.Height = 420;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var root = new StackPanel { Margin = new Thickness(12) };
            root.Children.Add(new TextBlock { Text = "Reventa segura dentro de la app", FontSize = 16, FontWeight = FontWeights.Bold });

            var grid = new Grid { Margin = new Thickness(0,8,0,0) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            
            var left = new StackPanel();
            left.Children.Add(new TextBlock { Text = "Reventas disponibles", FontWeight = FontWeights.Bold });
            lstResales = new ListBox { Height = 260 };
            left.Children.Add(lstResales);
            var btnBuy = new Button { Content = "Comprar reventa", Margin = new Thickness(0,8,0,0), Width = 140 };
            btnBuy.Click += BtnBuy_Click;
            left.Children.Add(btnBuy);
            Grid.SetColumn(left, 0);
            grid.Children.Add(left);

            
            var right = new StackPanel();
            right.Children.Add(new TextBlock { Text = "Mis tickets", FontWeight = FontWeights.Bold });
            cbMyTickets = new ComboBox { Height = 120 };
            right.Children.Add(cbMyTickets);
            right.Children.Add(new TextBlock { Text = "Precio de reventa" });
            txtPrice = new TextBox { Width = 120 };
            right.Children.Add(txtPrice);
            var btnCreate = new Button { Content = "Publicar reventa", Margin = new Thickness(0,8,0,0), Width = 140 };
            btnCreate.Click += BtnCreate_Click;
            right.Children.Add(btnCreate);
            Grid.SetColumn(right, 1);
            grid.Children.Add(right);

            root.Children.Add(grid);

            var btnClose = new Button { Content = "Cerrar", Width = 80, Margin = new Thickness(0,8,0,0), HorizontalAlignment = HorizontalAlignment.Right };
            btnClose.Click += (s, e) => this.Close();
            root.Children.Add(btnClose);

            this.Content = root;

            RefreshData();
        }

        private void RefreshData()
        {
            lstResales.Items.Clear();
            var resales = DataStore.GetAvailableResales();
            foreach (var r in resales)
            {
                var ticket = DataStore.GetTicketsForUser(r.Seller).FirstOrDefault(t => t.Id == r.TicketId);
                var ev = ticket?.Evento?.Nombre ?? "(evento)";
                lstResales.Items.Add(new ListBoxItem { Content = $"{ev} - ${r.Price} - Vendedor: {r.Seller}", Tag = r.Id });
            }

            cbMyTickets.Items.Clear();
            var myTickets = DataStore.GetTicketsForUser(usuario);
            foreach (var t in myTickets)
            {
                cbMyTickets.Items.Add(new ComboBoxItem { Content = $"{t.Evento.Nombre} - ${t.Precio} (ID: {t.Id})", Tag = t.Id });
            }
        }

        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            if (lstResales.SelectedItem is ListBoxItem it && it.Tag is Guid id)
            {
                var ok = DataStore.BuyResale(id, usuario);
                if (ok)
                {
                    MessageBox.Show("Compra de reventa exitosa.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("No se pudo completar la compra.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione una reventa.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (cbMyTickets.SelectedItem is ComboBoxItem it && it.Tag is Guid tid)
            {
                if (!decimal.TryParse(txtPrice.Text, out var price))
                {
                    MessageBox.Show("Precio inválido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var listing = DataStore.CreateResale(tid, usuario, price);
                if (listing != null)
                {
                    MessageBox.Show("Reventa publicada.", "Ok", MessageBoxButton.OK, MessageBoxImage.Information);
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("No se pudo publicar la reventa (precio > original o ticket no encontrado).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione uno de sus tickets.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
