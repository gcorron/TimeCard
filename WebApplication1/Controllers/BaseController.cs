using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeCard.Repo.Repos;

namespace TimeCard.Controllers
{
    public class BaseController : Controller
    {
        protected readonly string ConnString;
        protected readonly LookupRepo LookupRepo;
        private int _curUserId;
        protected int CurrentUserId
        {
            get
            {
                var user = LookupRepo.GetLookupByVal("Contractor",CurrentUsername);
                return user.Id;
            }
        }

        private string _curUsername;
        protected string CurrentUsername
        {
            get
            {
                var username = HttpContext.User.Identity.Name;
                _curUsername = username.Substring(username.IndexOf(@"\") + 1);
                return _curUsername;
            }
        }

        public BaseController()
        {
            ConnString = ConfigurationManager.ConnectionStrings["TimeCard"].ConnectionString;
            LookupRepo = new LookupRepo(ConnString);
        }
    }
}