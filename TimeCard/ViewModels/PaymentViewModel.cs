using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeCard.Domain;

namespace TimeCard.ViewModels
{
    public class PaymentViewModel
    {
        public IEnumerable<PaymentSummary> PaymentSummary { get; set; }
    }
}