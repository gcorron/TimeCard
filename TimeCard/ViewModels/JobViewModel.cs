using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeCard.Domain;

namespace TimeCard.ViewModels
{
    public class JobViewModel
    {
        public int ContractorId { get; set; }
        public IEnumerable<Job> Jobs { get; set; }
    }
}