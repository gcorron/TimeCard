using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCard.Domain
{
    public class PaymentSummary
    {
        public string Client { get; set; }
        public string Project { get; set; }
        public string BillType { get; set; }
        public decimal Billed { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get => Billed - Paid; }
    }
}
