using System;
namespace TimeCard.Domain
{
    public class Job
    {
        private DateTime BaselineDate = new DateTime(2018, 12, 22);
        public int JobId { get; set; }
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
        public int BillType { get; set; }
        public string BillTypeDescr { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public bool Active { get; set; }
        public decimal StartDay { get; set; }
        public DateTime? StartDate
        {
            get
            {
                if (StartDay == 0)
                {
                    return null;
                }

                int cycle = (int)Decimal.Floor(StartDay);
                return BaselineDate.AddDays((double)(cycle * 14 + (StartDay - cycle) * 100));
            }

            set
            {
                if (value == null || value < BaselineDate)
                {
                    StartDay = 0;
                }
                else
                {
                    int days = (value - BaselineDate).Value.Days;
                    StartDay = days / 14 + (days % 14) * (decimal)0.01;
                }
            }
        }
    }
}
