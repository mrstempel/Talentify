using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.Mvc;
using Telentify.Admin.Models;

namespace Telentify.Admin.Controllers
{
    public class CoachingController : BaseController
    {
		public ActionResult Index()
		{
			ViewBag.CoachingCount = UnitOfWork.CoachingRequestRepository.GetCoachingRequestCount();
			ViewBag.CompletedCount = UnitOfWork.CoachingRequestRepository.GetCompletedCount();
			ViewBag.RejectedCount = UnitOfWork.CoachingRequestRepository.GetRejectedCount();
			ViewBag.CancledCount = UnitOfWork.CoachingRequestRepository.GetCancledCount();

			return View();
        }

	    public ActionResult Details()
	    {
			var coachings = from c in UnitOfWork.CoachingRequestRepository.AsQueryable()
							join
								fromUser in UnitOfWork.BaseUserRepository.AsQueryable() on c.FromUserId equals fromUser.Id
							join toUser in UnitOfWork.BaseUserRepository.AsQueryable() on c.ToUserId equals toUser.Id
							join subject in UnitOfWork.SubjectCategoryRepository.AsQueryable() on c.SubjectCategoryId equals subject.Id
							orderby c.CreatedDate descending
							select new CoachingSummary
							{
								Request = c,
								FromUserName = fromUser.Firstname + " " + fromUser.Surname,
								ToUserName = toUser.Firstname + " " + toUser.Surname,
								Subject = subject.Name
							};

			return View(coachings);
	    }
    }
}
