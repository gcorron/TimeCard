﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TimeCard.Domain
{
    public class WorkExtended
    {
        private DateTime BaselineDate = new DateTime(2018, 12, 22);
        public int WorkId { get; set; }
        public int ContractorId { get; set; }
        public int JobId { get; set; }
        public string Job { get; set; }
        public decimal WorkDay { get; set; }
        public string Descr { get; set; }
        public decimal Hours { get; set; }
        public int Cycle { get => (int)Decimal.Floor(WorkDay); }
        public string CycleEndDate { get => $"{BaselineDate.AddDays((double)(Cycle * 14 + 13)): MM/dd/yy}"; }
        public DateTime WorkDate { get => BaselineDate.AddDays((double)(Cycle * 14 + (WorkDay - Cycle) * 100)); }
        public int WorkWeek { get => (WorkDay % 1) < (decimal)0.07 ? 0 : 1; }
        public DateTime WorkWeekDate { get => BaselineDate.AddDays((double)(Cycle * 14 + 7 * WorkWeek)); }
        public int WorkWeekDay { get => (int)((WorkDay % 1) * 100) % 7; }
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
        public string Client { get; set; }
        public string ClientCode { get; set; }
        public string Project { get; set; }
        public string BillType { get; set; }
        public string WorkType { get; set; }
        public string Contractor { get; set; }
    }
}
