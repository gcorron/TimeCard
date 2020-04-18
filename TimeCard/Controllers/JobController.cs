using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeCard.Repo.Repos;

namespace TimeCard.Controllers
{
    public class JobController : BaseController
    {
        private readonly JobRepo _JobRepo;

        public JobController()
        {
            _JobRepo = new JobRepo(ConnString);
        }

        public ActionResult Index()
        {
            int contractorId = LookupRepo.GetLookupByVal("Contractor", CurrentUsername).Id;
            var jobs = _JobRepo.GetJobStart(contractorId);
            return View(new ViewModels.JobViewModel{ ContractorId=contractorId, Jobs=jobs });
        }

        public void SetJobDate(int contractorId, int jobId, string theDate)
        {
            decimal startDay=0;
            if (!String.IsNullOrEmpty(theDate))
            {
                DateTime BaselineDate = new DateTime(2018, 12, 22);
                DateTime startDate = DateTime.Parse(theDate);
                int days = (startDate - BaselineDate).Days;
                startDay = days / 14 + (days % 14) * (decimal)0.01;
            }
            _JobRepo.UpdateJobStart(contractorId, jobId, startDay);

        }
    }
}