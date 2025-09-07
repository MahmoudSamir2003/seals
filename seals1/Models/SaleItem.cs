using System.ComponentModel.DataAnnotations.Schema;

namespace SalesSuite.Models
{
    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }      // FK
        public int ProductId { get; set; }   // FK
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }

        [NotMapped]
        public decimal LineTotal => (Quantity * UnitPrice) - Discount + Tax;

        // ✅ Navigation
        public Sale? Sale { get; set; }
        public Product? Product { get; set; }
    }
}
