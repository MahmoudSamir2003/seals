using System.ComponentModel.DataAnnotations.Schema;

namespace SalesSuite.Models
{
    [Table("PurchaseItems")]
    public class PurchaseItem
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }
        public Purchase Purchase { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Column("Quantity")]
        public int Quantity { get; set; }

        // العمود الفعلي في SQL = UnitCost
        [Column("UnitCost")]
        public decimal Price { get; set; }

        // ✅ LineCost عمود محسوب في SQL → للقراءة فقط
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("LineCost")]
        public decimal Total { get; private set; }
    }
}
