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
        public IEnumerable<TimeCardUnpaid> TimeCardsUnpaid { get; set; }
    }
}