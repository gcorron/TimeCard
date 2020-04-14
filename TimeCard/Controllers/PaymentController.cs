using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeCard.Repo.Repos;
using TimeCard.ViewModels;

namespace TimeCard.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly PaymentRepo _PaymentRepo;
        private readonly WorkRepo _WorkRepo;
        private readonly LookupRepo _LookupRepo;

        public PaymentController()
        {
            _PaymentRepo = new PaymentRepo(ConnString);
            _WorkRepo = new WorkRepo(ConnString);
            _LookupRepo = new LookupRepo(ConnString);
        }

        [HttpGet]
        public ActionResult Index()
        {

            var vm = new PaymentViewModel
            {
                SelectedContractor = CurrentUserId,
                IsAdmin=false,
                EditPayment = new Domain.Payment { ContractorId=CurrentUserId }
            };
            prepPayment(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(PaymentViewModel vm, string buttonValue)
        {
            switch(buttonValue)
            {
                case "Save":
                    if (ModelState.IsValid)
                    {
                        _PaymentRepo.SavePayment(vm.EditPayment);
                    }
                    break;
                default:
                    ModelState.Clear();
                    break;
            }
            prepPayment(vm);
            return PartialView("_EditPayment", vm);
        }

        private void prepPayment(PaymentViewModel vm)
        {
            vm.PaymentSummary = _PaymentRepo.GetSummary(CurrentUserId);
            vm.Jobs = _WorkRepo.GetJobs("- Select -").Select(x => new SelectListItem { Text = x.Descr, Value = x.Id.ToString() });
            vm.Contractors = _LookupRepo.GetLookups("Contractor", "- Select -").Select(x => new SelectListItem { Text = x.Descr, Value = x.Id.ToString() });
        }
    }
}