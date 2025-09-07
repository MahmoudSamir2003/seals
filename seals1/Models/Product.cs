using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesSuite.Models
{
    [Table("Products")]
    public class Product
    {
        public int Id { get; set; }

        // ✅ ربط Name بعمود ProductName
        [Column("Name")]
        public string Name { get; set; } = "";

        public string SKU { get; set; } = "";

        [Column("Barcode")]
        public string? Barcode { get; set; }

        [Column("CategoryId")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        [Column("Price")]
        public decimal Price { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }

        [Column("Description")]
        public string? Description { get; set; }

        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ✅ Relations
        public ICollection<PurchaseItem>? PurchaseItems { get; set; }
        public ICollection<SaleItem>? SaleItems { get; set; }
        public Inventory? Inventory { get; set; }
    }
}
