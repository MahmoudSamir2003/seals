// Services/ProductService.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesSuite.Models;
using SalesSuite.Utils;
using ZXing;

namespace SalesSuite.Services
{
    public class ProductService
    {
        public static string GenerateSku(string? prefix = null)
        {
            // مثال: CAT-ABC123
            var rnd = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("=", "").Replace("+", "").Replace("/", "");
            var code = new string(rnd.Take(6).ToArray()).ToUpper();
            return string.IsNullOrWhiteSpace(prefix) ? code : $"{prefix}-{code}";
        }

        public static string GenerateBarcodeValue(string sku) => $"P-{sku}";

        public async Task<Product> AddProductAsync(Product p)
        {
            using var db = new AppDb();
            if (string.IsNullOrWhiteSpace(p.SKU))
                p.SKU = GenerateSku();
            p.Barcode = GenerateBarcodeValue(p.SKU);
            p.CreatedAt = DateTime.Now;

            db.Products.Add(p);
            await db.SaveChangesAsync();

            // Ensure inventory row
            if (!await db.Inventory.AnyAsync(i => i.ProductId == p.Id))
            {
                db.Inventory.Add(new Inventory { ProductId = p.Id, Quantity = 0, MinQuantity = 0 });
                await db.SaveChangesAsync();
            }
            return p;
        }

        // توليد صورة باركود (WPF WriteableBitmap)
        public static System.Windows.Media.Imaging.WriteableBitmap CreateBarcodeBitmap(string value, int width = 300, int height = 100)
        {
            var writer = new ZXing.BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions { Width = width, Height = height, Margin = 1, PureBarcode = true }
            };
            var pixelData = writer.Write(value);
            var wb = new System.Windows.Media.Imaging.WriteableBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Gray8, null);
            wb.WritePixels(new System.Windows.Int32Rect(0, 0, width, height), pixelData.Pixels, pixelData.Width, 0);
            return wb;
        }
    }
}
