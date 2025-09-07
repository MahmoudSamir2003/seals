using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesSuite.Models
{
    [Table("Customers")]
    public class Customer
    {
        public int Id { get; set; }

        [Column("FullName")]
        public string FullName { get; set; } = "";

        [Column("Phone")]
        public string Phone { get; set; } = "";

        [Column("Email")]
        public string Email { get; set; } = "";

        [Column("Address")]
        public string Address { get; set; } = "";

        [Column("City")]
        public string City { get; set; } = "";

        [Column("Notes")]
        public string Notes { get; set; } = "";

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
