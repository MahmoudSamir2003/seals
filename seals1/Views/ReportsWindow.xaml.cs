using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using SalesSuite.Utils;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using SalesSuite.Models;
using System.Windows.Controls;

namespace SalesSuite.Views
{
    public partial class ReportsWindow : Window
    {
        public ReportsWindow()
        {
            InitializeComponent();
            LoadReports();
        }

        private void LoadReports()
        {
            using var db = new AppDb();

            // بيانات الرسم البياني
            var monthlyData = db.Purchases
                .GroupBy(p => new { p.Date.Year, p.Date.Month })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Total = g.Sum(x => (decimal?)x.Total) ?? 0
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();

            ReportsChart.Series = new ISeries[]
            {
                new ColumnSeries<decimal>
                {
                    Values = monthlyData.Select(x => x.Total).ToArray(),
                    Name = "Sales",
                    Fill = new SolidColorPaint(SKColors.CornflowerBlue)
                }
            };

            ReportsChart.XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = monthlyData
                        .Select(x => $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(x.Month)} {x.Year}")
                        .ToList(),
                    LabelsPaint = new SolidColorPaint(SKColors.Black)
                }
            };

            // بيانات الجدول
            var tableData = db.Purchases
                .OrderByDescending(p => p.Date)
                .Take(20)
                .Select(p => new
                {
                    Date = p.Date.ToShortDateString(),
                    Customer = p.Customer.FullName,
                    Total = p.Total
                })
                .ToList();

            ReportsGrid.ItemsSource = tableData;
        }

 
        // ✅ Export to PDF
        private void ExportToPdf_Click(object sender, RoutedEventArgs e)
        {
            var tableData = ReportsGrid.ItemsSource;
            if (tableData == null) return;

            var sfd = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = "Reports.pdf"
            };

            if (sfd.ShowDialog() == true)
            {
                var doc = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(30);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header().Text("📊 Sales Report")
                            .FontSize(18).SemiBold().AlignCenter();

                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.ConstantColumn(100);
                                cols.RelativeColumn();
                                cols.ConstantColumn(100);
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Date");
                                header.Cell().Element(CellStyle).Text("Customer");
                                header.Cell().Element(CellStyle).Text("Total");
                            });

                            // Rows
                            foreach (var record in tableData)
                            {
                                string date = record.GetType().GetProperty("Date")?.GetValue(record, null)?.ToString() ?? "";
                                string customer = record.GetType().GetProperty("Customer")?.GetValue(record, null)?.ToString() ?? "";
                                string total = record.GetType().GetProperty("Total")?.GetValue(record, null)?.ToString() ?? "";

                                table.Cell().Element(CellStyle).Text(date);
                                table.Cell().Element(CellStyle).Text(customer);
                                table.Cell().Element(CellStyle).Text(total);
                            }

                        });
                    });
                });

                doc.GeneratePdf(sfd.FileName);
                MessageBox.Show("✅ Exported to PDF successfully!");
            }
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container.PaddingVertical(5)
                           .PaddingHorizontal(5)
                           .BorderBottom(1)
                           .BorderColor(Colors.Grey.Lighten2);
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
