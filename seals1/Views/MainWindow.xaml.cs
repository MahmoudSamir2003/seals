using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SalesSuite.Models;
using SalesSuite.Utils;

namespace SalesSuite
{
    public partial class MainWindow : Window
    {
        private readonly User currentUser;

        public MainWindow(User u)
        {
            InitializeComponent();
            currentUser = u;
            Title = $"Dashboard - {u.Username}";
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            using var db = new AppDb();

            var totalSales = db.Purchases.Sum(p => (decimal?)p.Total) ?? 0;
            var lastMonth = db.Purchases
                .Where(p => p.Date.Month == System.DateTime.Now.AddMonths(-1).Month &&
                            p.Date.Year == System.DateTime.Now.AddMonths(-1).Year)
                .Sum(p => (decimal?)p.Total) ?? 0;
            var totalCustomers = db.Customers.Count();

            TotalSalesTxt.Text = $"${totalSales:F2}";
            LastMonthSalesTxt.Text = $"${lastMonth:F2}";
            TotalCustomersTxt.Text = totalCustomers.ToString();

            var monthlyData = db.Purchases
                .GroupBy(p => new { p.Date.Year, p.Date.Month })
                .Select(g => new { g.Key.Month, g.Key.Year, Total = g.Sum(x => (decimal?)x.Total) ?? 0 })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();

            SalesChart.Series = new ISeries[]
            {
                new ColumnSeries<decimal>
                {
                    Values = monthlyData.Select(x => x.Total).ToArray(),
                    Name = "Sales",
                    Fill = new SolidColorPaint(SKColors.DodgerBlue)
                }
            };

            SalesChart.XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = monthlyData.Select(x =>
                        $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x.Month)} {x.Year}"
                    ).ToList(),
                    LabelsPaint = new SolidColorPaint(SKColors.Black)
                }
            };

            var recents = db.Purchases
                .OrderByDescending(p => p.Date)
                .Take(5)
                .Select(p => new
                {
                    Date = p.Date.ToShortDateString(),
                    CustomerName = p.Customer.FullName,
                    Total = $"${p.Total:F2}"
                })
                .ToList();

            RecentSalesList.ItemsSource = recents;
        }

        // ✅ Fix: Now NavList works
        private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavList.SelectedItem is ListBoxItem item)
            {
                string tag = item.Tag.ToString();
                switch (tag)
                {
                    case "Customers":
                        new SalesSuite.Views.CustomersWindow().Show();
                        this.Close();
                        break;
                    case "Products":
                        new SalesSuite.Views.ProductsWindow().Show();
                        this.Close();
                        break;
                    case "Purchases":
                        new SalesSuite.Views.PurchasesWindow().Show();
                        this.Close();
                        break;
                    case "Reports":
                        new SalesSuite.Views.ReportsWindow().Show();
                        this.Close();
                        break;
                    case "Register":
                        new SalesSuite.Views.RegisterWindow().Show();
                        this.Close();
                        break;
                    case "Logout":
                        new SalesSuite.Views.LoginWindow().Show();
                        this.Close();
                        break;
                }
            }
        }
    }
}
