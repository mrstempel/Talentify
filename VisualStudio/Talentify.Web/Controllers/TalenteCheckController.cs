using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
	public class TalenteCheckController : BaseController
    {
		[AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}
