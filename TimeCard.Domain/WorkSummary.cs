using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeCard.Helpers;

namespace TimeCard.Domain
{
    public class WorkSummary
    {
        public int JobId { get; set; }
        public string Job { get; set; }
        public decimal WorkPeriod { get; set; }
        public decimal Hours { get; set; }
        public string WorkPeriodDescr { get => $"{DateRef.GetWorkDate(WorkPeriod): MM/dd/yyyy}"; }
    }
}
