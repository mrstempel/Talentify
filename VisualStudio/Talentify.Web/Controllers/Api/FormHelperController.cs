using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;
using Talentify.ORM.Utils;

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
			var myOffers = (from allSubjects in UnitOfWork.SubjectCategoryRepository.AsQueryable()
							  join offers in UnitOfWork.CoachingOfferRepository.AsQueryable() on allSubjects.Id equals offers.SubjectCategoryId
							where allSubjects.IsActive && offers.UserId == userId
							  orderby allSubjects.Sorter
							  select offers).Distinct();
			//var myOffers = UnitOfWork.CoachingOfferRepository.Get(c => c.UserId == userId, null, "SubjectCategory");
		    var myOffersWithCategory = new List<CoachingOffer>();
		    foreach (var offer in myOffers)
		    {
				myOffersWithCategory.Add(UnitOfWork.CoachingOfferRepository.GetById(offer.Id, "SubjectCategory"));
		    }
			return Json(myOffersWithCategory, JsonRequestBehavior.AllowGet);
	    }

		public JsonResult MyCoachingTimes(int userId)
		{
			var myTimes = UnitOfWork.CoachingTimeRepository.Get(i => i.UserId == userId).OrderBy(i => i.Day);
			return Json(myTimes, JsonRequestBehavior.AllowGet);
		}

	    public JsonResult SendMessage(int conversationId, int fromUserId, int toUserId, int targetId, string text)
	    {
		    var message = UnitOfWork.ConversationRepository.AddMessage(conversationId, fromUserId, toUserId, targetId, text);
			return Json(new { UserId = message.UserId, UserImage = message.UserImage, Username = message.Username, CreatedDate = message.CreatedDate.ToString("g"), Text = message.Text}, JsonRequestBehavior.AllowGet);
	    }

		[AllowAnonymous]
	    public ActionResult SubjectCategoryTags()
	    {
		    var allSubjects = UnitOfWork.SubjectCategoryRepository.Get(s => s.IsActive);
			ViewBag.Results = "[";
		    foreach (var s in allSubjects)
		    {
				ViewBag.Results += string.Format("[{0},\"{1}\",null,\"{1}\"],", s.Id, s.Name);
		    }
			ViewBag.Results = ViewBag.Results.ToString().Substring(0, ViewBag.Results.ToString().Length - 1);
			ViewBag.Results += "]";

			return View();
	    }

		[AllowAnonymous]
		public ActionResult SchoolSelect(string plzStart)
		{
			var allSchools = UnitOfWork.SchoolRepository.Get(s => s.IsActive && s.ZipCode.StartsWith(plzStart));
			ViewBag.Results = "[";
			foreach (var s in allSchools)
			{
				ViewBag.Results += string.Format("[{0},\"{1}\",null,\"{1}\"],", s.Id, s.Name.Replace("\"", ""));
			}
			ViewBag.Results = ViewBag.Results.ToString().Substring(0, ViewBag.Results.ToString().Length - 1);
			ViewBag.Results += "]";

			return View();
		}

		[AllowAnonymous]
		public ActionResult SearchSchoolForm()
		{

			var allSchholTypes = UnitOfWork.SchoolTypeRepository.Get().ToList();
			allSchholTypes.Insert(0, new SchoolType() { Id = 0, Code = "Schultyp"});
			ViewBag.AllSchoolTypes = new SelectList(allSchholTypes, "Id", "Code");
			return View(new School());
		}

		[AllowAnonymous]
	    public ActionResult SearchSchools(string bundesland, int schoolTypeId, string name, string address)
	    {
		    return View(UnitOfWork.SchoolRepository.SearchSchools(bundesland, schoolTypeId, name, address));
	    }

		[AllowAnonymous]
	    public JsonResult GetSchoolEmailSuffix(int schoolId)
	    {
			return Json(UnitOfWork.SchoolRepository.GetById(schoolId).EmailSuffix, JsonRequestBehavior.AllowGet);
	    }

		[AllowAnonymous]
		public ActionResult AddSchoolForm()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult AddSchoolForm(string name, string email, string school)
		{
			try
			{
				var mailMsg = new MailMessage(new MailAddress(email),
					new MailAddress(ConfigurationManager.AppSettings["Email.Feedback.To"]));
				mailMsg.Subject = "Neuer Schulvorschlag";
				mailMsg.Body = string.Format("{0} (Email: {1}) hat eine neue Schule vorgeschlagen: {2}", name, email, school);
				Email.Send(mailMsg);
			}
			catch (Exception) {}
			FormSuccess = new FormFeedback();
			return View();
		}

		[AllowAnonymous]
		public ActionResult AddSubjectForm()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult AddSubjectForm(string name, string email, string subject)
		{
			try
			{
				var mailMsg = new MailMessage(new MailAddress(email),
					new MailAddress(ConfigurationManager.AppSettings["Email.Feedback.To"]));
				mailMsg.Subject = "Neuer Schulfachvorschlag";
				mailMsg.Body = string.Format("{0} (Email: {1}) hat ein neues Schulfach vorgeschlagen: {2}", name, email, subject);
				Email.Send(mailMsg);
			}
			catch (Exception) { }
			FormSuccess = new FormFeedback();
			return View();
		}

	    public JsonResult AddSubjectLoggedUser(string subject)
	    {
			try
			{
				var mailMsg = new MailMessage(new MailAddress(LoggedUser.Email),
					new MailAddress(ConfigurationManager.AppSettings["Email.Feedback.To"]));
				mailMsg.Subject = "Neuer Schulfachvorschlag";
				mailMsg.Body = string.Format("{0} {1} (Email: {2}) hat ein neues Schulfach vorgeschlagen: {3}", LoggedUser.Firstname, LoggedUser.Surname, LoggedUser.Email, subject);
				Email.Send(mailMsg);
			}
			catch (Exception) { }
		    return Json(true, JsonRequestBehavior.AllowGet);
	    }

	    public ActionResult FeedbackForm()
	    {
		    return View();
	    }

		[HttpPost]
		public ActionResult FeedbackForm(string type, string feedback)
		{
			try
			{
				var mailMsg = new MailMessage(new MailAddress(LoggedUser.Email),
					new MailAddress(ConfigurationManager.AppSettings["Email.Feedback.To"]));
				mailMsg.Subject = "talentify.me Feedbackformular";
				mailMsg.Body = string.Format("Benutzer: {0} {1}, {2}", LoggedUser.Firstname, LoggedUser.Surname, LoggedUser.Email);
				mailMsg.Body += Environment.NewLine + Environment.NewLine;
				mailMsg.Body += string.Format("Feedback Typ: {0}", type);
				mailMsg.Body += Environment.NewLine;
				mailMsg.Body += string.Format("Feedback Nachricht: {0}", feedback);
				Email.Send(mailMsg);
			}
			catch (Exception ex) { }
			FormSuccess = new FormFeedback();
			return View();
		}
    }
}
