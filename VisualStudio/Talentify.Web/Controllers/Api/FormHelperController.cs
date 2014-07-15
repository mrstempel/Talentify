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

	    public JsonResult GetEventOpenSeats(int eventId)
	    {
		    return Json(UnitOfWork.EventRepository.GetOpenSeats(eventId), JsonRequestBehavior.AllowGet);
	    }

	    public JsonResult SendMessage(int conversationId, int fromUserId, int toUserId, int targetId, string text)
	    {
		    var message = UnitOfWork.ConversationRepository.AddMessage(conversationId, fromUserId, toUserId, targetId, text);
			return Json(new { UserId = message.UserId, UserImage = message.UserImage, Username = message.Username, CreatedDate = message.CreatedDate.ToString("g"), Text = message.Text}, JsonRequestBehavior.AllowGet);
	    }
    }
}
