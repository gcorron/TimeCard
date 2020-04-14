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
        public int SelectedJob { get; set; }
        public int SelectedContractor { get; set; }
        public bool CanEdit { get => !(SelectedJob == 0 || SelectedContractor == 0); }
        public bool IsAdmin { get; set; }
        public Payment EditPayment { get; set; }
    }
}