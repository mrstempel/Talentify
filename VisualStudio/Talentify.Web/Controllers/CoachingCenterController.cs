using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class CoachingCenterController : BaseController
    {
		public ActionResult Index()
        {
            return View();
        }

	    public ActionResult Filter(StatusType statusType)
	    {
		    var results = UnitOfWork.CoachingRequestRepository.GetByStatus(LoggedUser.Id, statusType);
		    return View(results);
	    }

	    public ActionResult Stream(int coachingRequestId)
	    {
		    var stream = UnitOfWork.CoachingRequestRepository.GetStream(coachingRequestId);
		    return View(stream);
	    }
    }
}
