using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeCard.Repo;
using TimeCard.Helpers;

namespace TimeCard.Controllers
{
    public class WorkController : BaseController
    {
        private readonly WorkRepo _WorkRepo;

        public WorkController()
        {
            _WorkRepo = new WorkRepo(ConnString);
        }

        public ActionResult Index()
        {

            var vm = new ViewModels.WorkViewModel();
            prepWork(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(ViewModels.WorkViewModel vm)
        {
            prepWork(vm);
            return View(vm);
        }

        private void prepWork(ViewModels.WorkViewModel vm)
        {
            if (Session["Contractor"] == null)
            {
                var contractor = LookupRepo.GetLookupByVal("Contractor", CurrentUsername);
                Session["Contractor"] = contractor;
            }

            var cycles = GetPayCycles();
            int cycle = int.Parse(cycles.First().Value);
            vm.Jobs = _WorkRepo.GetJobs().Select(x => new SelectListItem { Text = x.Descr, Value = x.Id.ToString() });
            vm.PayCycles = cycles;
            if (vm.SelectedCycle == 0)
            {
                vm.SelectedCycle = cycle;
            }
            vm.WorkEntries = _WorkRepo.GetWork((Session["Contractor"] as Domain.Lookup).Id, vm.SelectedCycle, true);
            if (vm.EditWork == null)
            {
                vm.EditWork = new Domain.Work();
            }
            vm.EditDays = GetEditDays(vm.SelectedCycle);
        }
        private IEnumerable<SelectListItem> GetEditDays(int thisCycle)
        {
            return Enumerable.Range(0, 13).Select(x => new SelectListItem { Text = DateRef.GetWorkDate(thisCycle + (decimal)x / 100, false), Value = (thisCycle + (decimal)x / 100).ToString() });
        }

        private IEnumerable<SelectListItem> GetPayCycles()
        {
            var thisCycle = decimal.Floor(DateRef.GetWorkDay(DateTime.Today));
            return Enumerable.Range(0, 15).Select(x => new SelectListItem { Text = DateRef.GetWorkDate(thisCycle - x), Value = (thisCycle - x).ToString() });
        }
    }
}