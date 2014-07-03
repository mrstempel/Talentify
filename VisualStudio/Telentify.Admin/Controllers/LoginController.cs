using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.Mvc;
using WebMatrix.WebData;

namespace Telentify.Admin.Controllers
{
	[AllowAnonymous]
    public class LoginController : BaseController
    {
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(string username, string password)
		{
			if (WebSecurity.Login(username, password))
			{
				return RedirectToAction("Index", "Home");
			}

			return Index();
		}

    }
}
