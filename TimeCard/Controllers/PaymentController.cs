using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeCard.Repo.Repos;

namespace TimeCard.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly PaymentRepo _PaymentRepo;

        public PaymentController()
        {
            _PaymentRepo = new PaymentRepo(ConnString);
        }
        public ActionResult Index()
        {
            var vm = new ViewModels.PaymentViewModel
            {
                PaymentSummary = _PaymentRepo.GetSummary(CurrentUserId)
            };
            return View(vm);
        }
    }
}