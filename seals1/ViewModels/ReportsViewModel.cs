// ViewModels/ReportsViewModel.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.EntityFrameworkCore;
using SalesSuite.Utils;

namespace SalesSuite.ViewModels
{
    public class ReportsViewModel
    {
        public ISeries[] DailySalesSeries { get; set; } = Array.Empty<ISeries>();
        public string[] DailySalesLabels { get; set; } = Array.Empty<string>();

        public ISeries[] TopProductsSeries { get; set; } = Array.Empty<ISeries>();
        public string[] TopProductsLabels { get; set; } = Array.Empty<string>();

        public async Task LoadAsync()
        {
            using var db = new AppDb();

            var daily = await db.Set<DailySalesRow>()
                .FromSqlRaw("SELECT [Day], TotalSales FROM dbo.vw_SalesDaily ORDER BY [Day]")
                .ToListAsync();

            DailySalesLabels = daily.Select(d => d.Day.ToString("MM-dd")).ToArray();
            DailySalesSeries = new[]
            {
                new ColumnSeries<decimal> { Values = daily.Select(d => d.TotalSales).ToArray() }
            };

            var top = await db.Set<TopProductRow>()
                .FromSqlRaw("SELECT TOP 10 Name, QtySold FROM dbo.vw_TopProducts ORDER BY QtySold DESC")
                .ToListAsync();

            TopProductsLabels = top.Select(t => t.Name).ToArray();
            TopProductsSeries = new[]
            {
                new RowSeries<decimal> { Values = top.Select(t => t.QtySold).ToArray() }
            };
        }

        public class DailySalesRow { public DateTime Day { get; set; } public decimal TotalSales { get; set; } }
        public class TopProductRow { public string Name { get; set; } = ""; public decimal QtySold { get; set; } }
    }
}
