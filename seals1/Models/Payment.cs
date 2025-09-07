using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SalesSuite.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = "Cash";
        public DateTime PaidDate { get; set; }
    }
}