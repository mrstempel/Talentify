using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using Talentify.ORM.Mvc;
using WebMatrix.WebData;

namespace Talentify.Web.Controllers.Api
{
	[AllowAnonymous]
    public class AuthController : BaseController
    {
		public JsonResult Login(string email, string password)
		{
			var loginSuccess = false;
			try
			{
				if (WebSecurity.Login(email, password))
				{
					loginSuccess = true;
				}
			}
			catch (Exception ex) {}


			return Json(loginSuccess, JsonRequestBehavior.AllowGet);
        }

		public JsonResult Logout()
		{
			WebSecurity.Logout();
			this.WebContext.User = null;
			return Json(true, JsonRequestBehavior.AllowGet);
		}
    }
}
