using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers.Api
{
    public class FormHelperController : BaseController
    {
	    public override bool KeepSearchSessionAlive
	    {
		    get { return true; }
	    }

	    public JsonResult SchoolClasses(int schoolId)
        {
			return Json(UnitOfWork.SchoolRepository.GetClasses(schoolId), JsonRequestBehavior.AllowGet);
        }

	    public JsonResult MyCoachingOffers(int userId)
	    {
			var myOffers = UnitOfWork.CoachingOfferRepository.Get(c => c.UserId == userId, null, "SubjectCategory");
		    var myOffersWithCategory = new List<CoachingOffer>();
		    foreach (var offer in myOffers)
		    {
				myOffersWithCategory.Add(UnitOfWork.CoachingOfferRepository.GetById(offer.Id, "SubjectCategory"));
		    }
			return Json(myOffersWithCategory, JsonRequestBehavior.AllowGet);
	    }

	    public JsonResult SendMessage(int conversationId, int fromUserId, int toUserId, int targetId, string text)
	    {
		    var message = UnitOfWork.ConversationRepository.AddMessage(conversationId, fromUserId, toUserId, targetId, text);
			return Json(new { UserId = message.UserId, UserImage = message.UserImage, Username = message.Username, CreatedDate = message.CreatedDate.ToString("g"), Text = message.Text}, JsonRequestBehavior.AllowGet);
	    }

		[AllowAnonymous]
	    public ActionResult SubjectCategoryTags()
	    {
		    var allSubjects = UnitOfWork.SubjectCategoryRepository.Get();
			ViewBag.Results = "[";
		    foreach (var s in allSubjects)
		    {
				ViewBag.Results += string.Format("[{0},\"{1}\",null,\"{1}\"],", s.Id, s.Name);
		    }
			ViewBag.Results = ViewBag.Results.ToString().Substring(0, ViewBag.Results.ToString().Length - 1);
			ViewBag.Results += "]";

			return View();
	    }
    }
}
