using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.Mvc;
using Talentify.ORM.Mvc.Security;

namespace Talentify.Web.Controllers
{
	[RequireActiveSchool]
    public class CoachingCenterController : BaseController
    {
		public ActionResult Index(int id = 0)
		{
			ViewBag.RequestId = id;
			ViewBag.CoachingRequest = -1;
			if (id != 0)
			{
				ViewBag.CoachingRequestStatus = (int)UnitOfWork.CoachingRequestRepository.GetStatusType(id);
			}
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
					message = "hat Lernhilfe bewertet";
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

		public JsonResult CancelRequest(int coachingRequestId, string reason)
		{
			var statusMessage = "hat Lernhilfe nicht bestätigt. Grund: " + reason;
			var notificationMessage = string.Format("Lernhilfe nicht bestätigt von: {0} {1}", LoggedUser.Firstname, LoggedUser.Surname);
			var status = StatusType.Canceled;

			if (string.IsNullOrEmpty(reason))
			{
				statusMessage = "hat Lernanfrage abgelehnt";
				notificationMessage = string.Format("Lernanfrage abgelehnt von: {0} {1}", LoggedUser.Firstname, LoggedUser.Surname);
				status = StatusType.Rejected;
			}

			var newStatus = UnitOfWork.CoachingRequestRepository.UpdateStatus(coachingRequestId, status, LoggedUser, statusMessage, notificationMessage);

			if (newStatus != null)
			{
				var message = (string.IsNullOrEmpty(reason)) ? "hat Lernanfrage abgelehnt" : "hat Lernhilfe nicht bestätigt. Grund: " + reason;

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

			return Json(new { error = true }, JsonRequestBehavior.AllowGet);
		}

	    public ActionResult CompleteCoachingRequest(int coachingRequestId, string status)
	    {
		    ViewBag.Status = status;
		    ViewBag.IsConflicted = false;
		    var coachingRequest = UnitOfWork.CoachingRequestRepository.GetById(coachingRequestId);

			// check if status is conflicted
		    if (coachingRequest.StatusHistory.Count == 4 && 
				coachingRequest.StatusHistory.FirstOrDefault(s => s.StatusType == StatusType.Canceled) != null && 
				coachingRequest.StatusHistory.FirstOrDefault(s => s.StatusType == StatusType.Completed) != null)
		    {
			    ViewBag.IsConflicted = true;
		    }

			return View(coachingRequest);
	    }

	    public JsonResult SetCoachingRequestRating(int coachingRequestId, int val1, int val2, int val3)
	    {
			return Json(UnitOfWork.CoachingRequestRepository.SetCoachingRequestRating(coachingRequestId, val1, val2, val3, LoggedUser), JsonRequestBehavior.AllowGet);
	    }

		public JsonResult SetCoachingRequestRatingWithDate(int coachingRequestId, int val1, int val2, int val3, DateTime date, decimal duration, int payedPrice)
		{
			return Json(UnitOfWork.CoachingRequestRepository.SetCoachingRequestRating(coachingRequestId, val1, val2, val3, LoggedUser, date, duration, payedPrice), JsonRequestBehavior.AllowGet);
		}
    }
}
