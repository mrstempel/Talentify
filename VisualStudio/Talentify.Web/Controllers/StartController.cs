using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Talentify.ORM.FrontendLogic;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class StartController : BaseController
	{
		#region Members & Properties

		protected DateTime? LastStreamItemDate
	    {
		    get
		    {
			    if (Session["LastStreamItemDate"] != null)
				    return Session["LastStreamItemDate"] as DateTime?;
			    
				return null;
		    }
			set { Session["LastStreamItemDate"] = value; }
	    }

		#endregion

		public ActionResult Index()
        {
			if (Session["IsFirstLogin"] != null && Convert.ToBoolean(Session["IsFirstLogin"]))
				ViewBag.IsFirstLogin = true;

	        LastStreamItemDate = null;
	        Session["IsFirstLogin"] = null;
	        var student = UnitOfWork.StudentRepository.GetById(LoggedUser.Id);
	        ViewBag.Latitude = student.School.Latitude;
	        ViewBag.Longitude = student.School.Longitude;
	        ViewBag.AllSchools = UnitOfWork.SchoolRepository.GetSchoolsWithInfo();
            return View(new SearchParams());
        }

	    public ActionResult Stream()
	    {
		    var streamProvider = new StreamProvider(UnitOfWork);
		    var stream = streamProvider.GetStream(LastStreamItemDate);

		    if (stream != null)
			    LastStreamItemDate = stream.Last().Time;
			
			return View(stream);
	    }

	    public ActionResult Events()
	    {
		    return View(UnitOfWork.EventRepository.GetOnlineEvents().Take(3));
	    }
    }
}
