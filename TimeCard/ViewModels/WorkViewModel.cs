﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeCard.Domain;

namespace TimeCard.ViewModels
{
    public class WorkViewModel
    {
        public IEnumerable<SelectListItem> PayCycles { get; set; }
        public IEnumerable<SelectListItem> Jobs { get; set; }
        public IEnumerable<SelectListItem> EditDays { get; set; }
        public decimal TestWorkDay { get; set; }
        public string TestToday { get; set; }
        public IEnumerable<Work> WorkEntries { get;set; }
        public int SelectedCycle { get; set; }
        public Work EditWork { get; set; }

    }
}