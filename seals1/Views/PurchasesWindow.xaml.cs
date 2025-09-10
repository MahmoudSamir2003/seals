using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SalesSuite.Models;

namespace SalesSuite.Views
{
    public partial class PurchasesWindow : Window
    {
        private ObservableCollection<PurchaseItem> cart = new();

        public PurchasesWindow()
        {
            InitializeComponent();
            LoadCustomers();
            LoadProducts();
            GridItems.ItemsSource = cart;
        }

        private void LoadCustomers()
        {
            using var db = new Utils.AppDb();
            CustomerBox.ItemsSource = db.Customers.ToList();
            CustomerBox.DisplayMemberPath = "FullName";
        }

        private void LoadProducts()
        {
            using var db = new Utils.AppDb();
            ProductBox.ItemsSource = db.Products.ToList();
            ProductBox.DisplayMemberPath = "Name";
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (ProductBox.SelectedItem is not Product p)
            {
                MessageBox.Show("⚠️ Select a product");
                return;
            }
            if (!int.TryParse(QtyBox.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("⚠️ Enter valid quantity");
                return;
            }

            cart.Add(new PurchaseItem
            {
                ProductId = p.Id,
                Product = p,
                Price = p.Price,
                Quantity = qty
                // ❌ Total مش هيتكتب هنا لأنه بيتحسب في SQL
            });

            UpdateTotals();
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (GridItems.SelectedItem is PurchaseItem selected)
            {
                cart.Remove(selected);
                UpdateTotals();
            }
            else
            {
                MessageBox.Show("⚠️ Select an item to delete.");
            }
        }

        private void UpdateTotals()
        {
            // ⚠️ بما إن Total عمود محسوب في SQL
            // نحسبه يدويًا للعرض في الواجهة
            decimal subtotal = cart.Sum(i => i.Price * i.Quantity);
            decimal vat = subtotal * 0.15m;
            decimal grand = subtotal + vat;

            SubtotalTxt.Text = subtotal.ToString("F2");
            VatTxt.Text = vat.ToString("F2");
            GrandTotalTxt.Text = grand.ToString("F2");
        }

        private void SavePurchase_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerBox.SelectedItem is not Customer c)
            {
                MessageBox.Show("⚠️ Select a customer");
                return;
            }
            if (!cart.Any())
            {
                MessageBox.Show("⚠️ No items in purchase");
                return;
            }

            using var db = new Utils.AppDb();

            var purchase = new Purchase
            {
                CustomerId = c.Id,
                Date = DateTime.Now,
                Total = cart.Sum(i => i.Price * i.Quantity) * 1.15m
            };

            db.Purchases.Add(purchase);
            db.SaveChanges();

            foreach (var item in cart)
            {
                db.PurchaseItems.Add(new PurchaseItem
                {
                    PurchaseId = purchase.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                    // ❌ Total مش هيتكتب لأنه بيتحسب في SQL
                });

                var prod = db.Products.FirstOrDefault(x => x.Id == item.ProductId);
                if (prod != null) prod.Quantity -= item.Quantity;
            }

            db.SaveChanges();
            MessageBox.Show("✅ Purchase saved successfully with 15% VAT!");
            cart.Clear();
            UpdateTotals();
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
