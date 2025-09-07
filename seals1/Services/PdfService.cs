using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SalesSuite.Models;
using SalesSuite.Utils;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SalesSuite.Services
{
    public class PdfService
    {
        public async Task<string> ExportProductsCatalogAsync(string outputPath)
        {
            using var db = new AppDb();
            var products = await db.Products.OrderBy(p => p.Name).ToListAsync();
            QuestPDF.Settings.License = LicenseType.Community;

            var file = System.IO.Path.Combine(outputPath, $"Products_{DateTime.Now:yyyyMMdd_HHmm}.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    // تعديل AlignCenter
                    page.Header()
                        .AlignCenter()
                        .Text("كتالوج المنتجات")
                        .FontSize(20)
                        .SemiBold();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(4);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(3);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Text("المنتج");
                            h.Cell().Text("الكود");
                            h.Cell().Text("السعر");
                            h.Cell().Text("الفئة");
                        });

                        foreach (var p in products)
                        {
                            table.Cell().Text(p.Name);
                            table.Cell().Text(p.SKU);
                            table.Cell().Text(p.Price.ToString("0.00"));
                            table.Cell().Text(p.CategoryId?.ToString() ?? "-");
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(txt =>
                        {
                            txt.Span("تم التوليد: ").SemiBold();
                            txt.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                        });
                });
            }).GeneratePdf(file);

            return file;
        }
    }
}
