using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesSuite.Models
{
    [Table("Purchases")]
    public class Purchase
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        [Column("Quantity")]
        public int? Quantity { get; set; }

        // ✅ العمود موجود فعليًا في SQL باسم Total
        [Column("Total")]
        public decimal? Total { get; set; }

        [Column("Date")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Column("SupplierName")]
        public string? SupplierName { get; set; }

        [Column("PurchaseDate")]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        [Column("TotalCost")]
        public decimal TotalCost { get; set; }

        [Column("Notes")]
        public string? Notes { get; set; }

        public ICollection<PurchaseItem>? PurchaseItems { get; set; }
    }
}
