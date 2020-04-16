using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCard.Domain
{
    public class TimeCardUnpaid
    {
        private DateTime BaselineDate = new DateTime(2018, 12, 22);
        public decimal WorkDay { get; set; }
        public decimal Hours { get; set; }
        public override string ToString()
        {
            
            int cycle = (int)Decimal.Floor(WorkDay);
            if (cycle == 0)
            {
                return "- Select -";
            }
            else
            {
                return $"{BaselineDate.AddDays((double)(cycle * 14 + (WorkDay - cycle) * 100)):MM/dd/yyyy} {Hours:n2}";
            }
        }
    }
}
