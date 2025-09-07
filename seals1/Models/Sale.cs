using System;
using System.Collections.Generic;

namespace SalesSuite.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public int? CustomerId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public string? Notes { get; set; }
        public int? CreatedByUserId { get; set; }

        // ✅ Relations
        public Customer? Customer { get; set; }
        public ICollection<SaleItem>? SaleItems { get; set; }
    }
}
