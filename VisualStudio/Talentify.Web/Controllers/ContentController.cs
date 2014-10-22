using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KwIt.Project.Pattern.DAL.Context;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class ContentController : BaseController
    {
		[AllowAnonymous]
        public ActionResult Impressum()
        {
            return View();
        }

		[AllowAnonymous]
	    public ActionResult Datenschutz()
	    {
		    return View();
	    }

		[AllowAnonymous]
		public ActionResult Agb()
		{
			return View();
		}
    }
}
