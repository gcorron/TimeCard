using System;
namespace TimeCard.Domain
{
    public class Work
    {
        private DateTime BaselineDate = new DateTime(2018,12,22);
        public int WorkId { get; set; }
        public int ContractorId { get; set; }
        public int JobId { get; set; }
        public string Job { get; set; }
        public decimal WorkDay { get; set; }
        public string Descr { get; set; }
        public decimal Hours { get; set; }
        public string WorkDate
        {
            get
            {
                int cycle = (int)Decimal.Floor(WorkDay);
                return $"{BaselineDate.AddDays((double)(cycle * 14 + (WorkDay - cycle) * 100)) : MM/dd}";
            }
        }
    }
}
