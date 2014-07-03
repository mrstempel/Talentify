using System;
using System.Linq;
using System.Web.Mvc;
using Talentify.ORM.FrontendLogic;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class StartController : BaseController
    {
        public ActionResult Index()
        {
			if (Session["IsFirstLogin"] != null && Convert.ToBoolean(Session["IsFirstLogin"]))
				ViewBag.IsFirstLogin = true;

	        Session["IsFirstLogin"] = null;
            return View(new SearchParams());
        }

	    public ActionResult Stream()
	    {
		    var streamProvider = new StreamProvider(UnitOfWork);
		    var stream = streamProvider.GetStream();
			return View(stream);
	    }

	    public ActionResult Events()
	    {
		    return View(UnitOfWork.EventRepository.GetOnlineEvents().Take(5));
	    }
    }
}
