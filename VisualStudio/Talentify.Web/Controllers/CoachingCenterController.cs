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
		public ActionResult Index(int id = 0)
		{
			ViewBag.RequestId = id;
            return View();
        }

	    public ActionResult Filter(StatusType statusType, int requestId)
	    {
		    var results = UnitOfWork.CoachingRequestRepository.GetByStatus(LoggedUser.Id, statusType);
		    ViewBag.RequestId = requestId;
		    return View(results);
	    }

	    public ActionResult Stream(int coachingRequestId)
	    {
		    var stream = UnitOfWork.CoachingRequestRepository.GetStream(coachingRequestId);
		    return View(stream);
	    }

	    public JsonResult UpdateStatus(int coachingRequestId, StatusType status)
	    {
		    var newStatus = UnitOfWork.CoachingRequestRepository.UpdateStatus(coachingRequestId, status, LoggedUser);

		    if (newStatus != null)
		    {
				var message = "hat deine Lernanfrage bestätigt";

				if (status == StatusType.Completed)
				{
					message = "hat die Lernanfrage bewertet";
				}

				return Json(
					new
					{
						error = false,
						status = new
						{
							type = newStatus.StatusType.ToString(),
							username = string.Format("{0} {1}", LoggedUser.Firstname, LoggedUser.Surname),
							userId = LoggedUser.Id,
							image = LoggedUser.ProfileImageSmall,
							message,
							date = newStatus.CreatedDate
						}
					}, JsonRequestBehavior.AllowGet);
		    }

		    return Json(new {error = true}, JsonRequestBehavior.AllowGet);
	    }
    }
}
