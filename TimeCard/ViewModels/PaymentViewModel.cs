using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeCard.Domain;

namespace TimeCard.ViewModels
{
    public class PaymentViewModel
    {
        private DateTime BaselineDate = new DateTime(2018, 12, 22);
        public IEnumerable<PaymentSummary> PaymentSummary { get; set; }
        public IEnumerable<SelectListItem> Jobs { get; set; }
        public IEnumerable<SelectListItem> Contractors { get; set; }
        public int SelectedJobId { get; set; }
        public bool JobIsTimeCard { get; set; }
        public int SelectedContractorId { get; set; }
        public bool CanEdit { get => !(SelectedJobId == 0 || SelectedContractorId == 0); }
        public bool IsAdmin { get; set; }
        public Payment EditPayment { get; set; }
        public IEnumerable<Payment>Payments { get; set; }
        public IEnumerable<SelectListItem> TimeCardsUnpaid { get; set; }
        public decimal PaidThruWorkDay { get; set; }
        public string WorkDate(decimal workDay)
        {
            if (workDay == 0 )
            {
                return null;
            }
            int cycle = (int)Decimal.Floor(workDay);
            return $"{BaselineDate.AddDays((double)(cycle * 14 + (workDay - cycle) * 100)): MM/dd/yyyy}";
        }
    }
}