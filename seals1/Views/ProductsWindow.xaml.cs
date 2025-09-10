using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using SalesSuite.Models;
using SalesSuite.Services;
using SalesSuite.Utils;

namespace SalesSuite.Views
{
    public partial class ProductsWindow : Window
    {
        private readonly ProductService service = new();

        public ProductsWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            using var db = new AppDb();

            try
            {
                GridProducts.ItemsSource = db.Products
                                             .AsNoTracking()
                                             .OrderBy(p => p.Name) // ✅ مربوط بالعمود Name
                                             .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Database error: " + ex.Message);
            }
        }

        private void GenSku_Click(object sender, RoutedEventArgs e)
        {
            SkuBox.Text = ProductService.GenerateSku();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(PriceBox.Text, out var price))
            {
                MessageBox.Show("⚠️ Invalid Price");
                return;
            }

            if (!int.TryParse(QtyBox.Text, out var qty))
            {
                MessageBox.Show("⚠️ Invalid Quantity");
                return;
            }

            var p = new Product
            {
                Name = NameBox.Text.Trim(),
                SKU = SkuBox.Text.Trim(),
                Price = price,
                Quantity = qty,
                Description = DescBox.Text.Trim()
            };

            await service.AddProductAsync(p);
            LoadData();
            MessageBox.Show("✅ Product saved successfully!");
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (GridProducts.SelectedItem is not Product selectedProduct)
            {
                MessageBox.Show("⚠️ Please select a product to delete.");
                return;
            }

            if (MessageBox.Show($"Are you sure you want to delete product '{selectedProduct.Name}'?",
                                "Confirm Delete",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            using var db = new AppDb();

            try
            {
                var inv = db.Inventory.FirstOrDefault(i => i.ProductId == selectedProduct.Id);
                if (inv != null) db.Inventory.Remove(inv);

                var purchaseItems = db.PurchaseItems.Where(pi => pi.ProductId == selectedProduct.Id).ToList();
                if (purchaseItems.Any()) db.PurchaseItems.RemoveRange(purchaseItems);

                var saleItems = db.SaleItems.Where(si => si.ProductId == selectedProduct.Id).ToList();
                if (saleItems.Any()) db.SaleItems.RemoveRange(saleItems);

                db.Products.Remove(selectedProduct);

                db.SaveChanges();
                LoadData();

                MessageBox.Show("❌ Product and related records deleted successfully.",
                                "Deleted",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Error while deleting: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        private void PrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (GridProducts.SelectedItem is not Product p)
            {
                MessageBox.Show("⚠️ Select a product first");
                return;
            }

            WriteableBitmap bmp = ProductService.CreateBarcodeBitmap(p.Barcode ?? p.SKU, 400, 150);
            var img = new Image { Source = bmp, Width = 400, Height = 150 };

            PrintDialog pd = new();
            if (pd.ShowDialog() == true)
            {
                pd.PrintVisual(img, $"Barcode_{p.SKU}");
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
