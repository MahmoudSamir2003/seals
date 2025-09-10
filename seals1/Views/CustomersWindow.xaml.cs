using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using SalesSuite.Models;
using SalesSuite.Utils;

namespace SalesSuite.Views
{
    public partial class CustomersWindow : Window
    {
        public CustomersWindow()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            using var db = new AppDb();
            GridCustomers.ItemsSource = db.Customers.AsNoTracking().ToList();
        }

        private void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InputFullName.Text) || string.IsNullOrWhiteSpace(InputPhone.Text))
            {
                MessageBox.Show("Full Name and Phone are required!");
                return;
            }

            using var db = new AppDb();
            var c = new Customer
            {
                FullName = InputFullName.Text.Trim(),
                Phone = InputPhone.Text.Trim(),
                Email = InputEmail.Text.Trim(),
                Address = InputAddress.Text.Trim(),
                City = InputCity.Text.Trim(),
                Notes = InputNotes.Text.Trim(),
                CreatedAt = DateTime.Now
            };

            db.Customers.Add(c);
            db.SaveChanges();
            LoadCustomers();
            MessageBox.Show("✅ Customer saved successfully.");

            InputFullName.Clear();
            InputPhone.Clear();
            InputEmail.Clear();
            InputAddress.Clear();
            InputCity.Clear();
            InputNotes.Clear();
        }

        private void DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (GridCustomers.SelectedItem is not Customer selectedCustomer)
            {
                MessageBox.Show("Select a customer to delete.");
                return;
            }

            if (MessageBox.Show($"Delete {selectedCustomer.FullName}?",
                                "Confirm", MessageBoxButton.YesNo,
                                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                using var db = new AppDb();
                db.Customers.Remove(selectedCustomer);
                db.SaveChanges();
                LoadCustomers();
                MessageBox.Show("❌ Customer deleted.");
            }
        }
        private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavList.SelectedItem is ListBoxItem item)
            {
                string tag = item.Tag.ToString();
                switch (tag)
                {
                    case "Home":
                        new MainWindow(new User { Username = "Admin" }).Show();
                        this.Close();
                        break;
                    case "Customers":
                        new CustomersWindow().Show();
                        this.Close();
                        break;
                    case "Products":
                        new ProductsWindow().Show();
                        this.Close();
                        break;
                    case "Purchases":
                        new PurchasesWindow().Show();
                        this.Close();
                        break;
                    case "Reports":
                        new ReportsWindow().Show();
                        this.Close();
                        break;
                    case "Register":
                        new RegisterWindow().ShowDialog();
                        break;
                    case "Logout":
                        new LoginWindow().Show();
                        this.Close();
                        break;
                }
            }
        }

    }
}
