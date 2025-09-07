using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesSuite.Models
{
    [Table("Inventory")]
    public class Inventory
    {
        [Key]   // ✅ ده هو الـ Primary Key
        public int ProductId { get; set; }

        public decimal Quantity { get; set; }
        public decimal MinQuantity { get; set; }

        // Navigation
        public Product? Product { get; set; }
    }
}
