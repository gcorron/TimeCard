using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TimeCard.ViewModels
{
    public class WorkViewModel
    {
        public IEnumerable<SelectListItem> PayCycles { get; set; }
        public IEnumerable<SelectListItem> Jobs { get; set; }
        public int ContractorId { get; set; }
    }
}