using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesSuite.Models
{
    public class ReportRow
    {
        public string Date { get; set; } = "";
        public string Customer { get; set; } = "";
        public decimal Total { get; set; }
    }
}
