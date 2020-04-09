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
        public ActionResult Index(ViewModels.WorkViewModel vm, string buttonValue)
        {
            if (buttonValue == "Save")
            {
                if (ModelState.IsValid)
                {
                    var work = vm.EditWork;
                    _WorkRepo.SaveWork(work);
                }
            }
            else
            {
                if (buttonValue == "Delete")
                {
                    _WorkRepo.DeleteWork(vm.EditWork.WorkId);
                    vm.EditWork = null;
                }
                ModelState.Clear();
            }
            prepWork(vm);
            return View(vm);
        }

        private void prepWork(ViewModels.WorkViewModel vm)
        {
            Domain.Lookup contractor = Session["Contractor"] as Domain.Lookup;
            if (contractor == null)
            {
                contractor = LookupRepo.GetLookupByVal("Contractor", CurrentUsername);
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
                vm.EditWork = new Domain.Work { ContractorId=contractor.Id, WorkDay=DateRef.GetWorkDay(DateTime.Today) };
            }
            vm.EditDays = GetEditDays(vm.SelectedCycle);
        }
        private IEnumerable<SelectListItem> GetEditDays(int thisCycle)
        {
            return Enumerable.Range(0, 14).Select(x => new SelectListItem { Text = DateRef.GetWorkDate(thisCycle + (decimal)x / 100, false), Value = (thisCycle + (decimal)x / 100).ToString() });
        }

        private IEnumerable<SelectListItem> GetPayCycles()
        {
            var thisCycle = decimal.Floor(DateRef.GetWorkDay(DateTime.Today));
            return Enumerable.Range(0, 15).Select(x => new SelectListItem { Text = DateRef.GetWorkDate(thisCycle - x), Value = (thisCycle - x).ToString() });
        }
    }
}