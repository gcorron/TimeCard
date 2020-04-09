using System;
using System.ComponentModel.DataAnnotations;

namespace TimeCard.Domain
{
    public class Work
    {
        private DateTime BaselineDate = new DateTime(2018,12,22);
        public int WorkId { get; set; }
        [Range(1,int.MaxValue,ErrorMessage = "Contractor ID required")]
        public int ContractorId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Job")]
        public int JobId { get; set; }
        public string Job { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select a Date")]
        public decimal WorkDay { get; set; }
        [Required]
        public string Descr { get; set; }
        [Range(0.25, 8, ErrorMessage = "Hours Not Valid")]
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
