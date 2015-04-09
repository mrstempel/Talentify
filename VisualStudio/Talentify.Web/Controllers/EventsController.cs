using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Feedback;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class EventsController : BaseController
    {
		[AllowAnonymous]
		public ActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Filter(string filter, bool isFilter)
		{
			ViewBag.RegisteredIds = IsAuthenticated ? UnitOfWork.EventRepository.GetUserRegisteredEventIds(LoggedUser.Id) : new List<int>();
			return View(UnitOfWork.EventRepository.Filter(filter, (IsAuthenticated) ? LoggedUser.Id : 0));
		}

	    [AllowAnonymous]
	    public ActionResult Detail(int id)
	    {
			var userId = LoggedUser != null ? LoggedUser.Id : 0;
		    var reg =
			    UnitOfWork.EventRegistrationRepository.AsQueryable().FirstOrDefault(r => r.UserId == userId && r.EventId == id && !r.IsSignedOff);
			ViewBag.IsUserRegistered = reg != null;
		    ViewBag.IsConfirmed = (reg != null) && reg.Confirmed;
			ViewBag.NextEvents = UnitOfWork.EventRepository.GetNextEvents(id);
		    ViewBag.IsFirst = UnitOfWork.EventRegistrationRepository.GetRegisterCount(userId) == 0;
		    return View(UnitOfWork.EventRepository.GetById(id));
	    }

		[AllowAnonymous]
		public ActionResult DetailUrl(string detailUrl, bool? isFilter)
		{
			if (isFilter.HasValue && isFilter.Value)
			{
				ViewBag.RegisteredIds = UnitOfWork.EventRepository.GetUserRegisteredEventIds(LoggedUser.Id);
				return View("Filter", UnitOfWork.EventRepository.Filter(detailUrl, LoggedUser.Id));
			}
			
			var eventDetail = UnitOfWork.EventRepository.AsQueryable().FirstOrDefault(e => e.DetailUrl == detailUrl);

			var userId = LoggedUser != null ? LoggedUser.Id : 0;
			var reg =
				UnitOfWork.EventRegistrationRepository.AsQueryable().FirstOrDefault(r => r.UserId == userId && r.EventId == eventDetail.Id && !r.IsSignedOff);

			if (eventDetail != null)
			{
				ViewBag.IsUserRegistered = reg != null;
				ViewBag.IsConfirmed = (reg != null) && reg.Confirmed;
				ViewBag.NextEvents = UnitOfWork.EventRepository.GetNextEvents(eventDetail.Id);
				ViewBag.IsFirst = (!IsAuthenticated) || UnitOfWork.EventRegistrationRepository.GetRegisterCount(userId) == 0;
			}

			return View("Detail", eventDetail);
		}

	    public ActionResult Register(int id)
	    {
		    var updtodateUser = UnitOfWork.BaseUserRepository.AsQueryable().FirstOrDefault(u => u.Id == LoggedUser.Id);
			if (updtodateUser != null && updtodateUser.IsWorkshopBlocked)
		    {
			    return View("Blocked");
		    }

		    var eventDetail = UnitOfWork.EventRepository.GetById(id);
			var myBonus = UnitOfWork.BonuspointRepository.GetUserBonus(LoggedUser.Id);
			// default
			var payOption = "pay-or-set";
			// check if bonus is lower than needed
		    if (myBonus < eventDetail.Bonuspoints)
				payOption = "pay";
			// check if it is first eventregistration
		    var registerCount = UnitOfWork.EventRegistrationRepository.GetRegisterCount(LoggedUser.Id);
		    if (registerCount == 0)
			    payOption = "first";
			// check if premium member

		    ViewBag.PayOption = payOption;
		    ViewBag.MyBonus = myBonus;
		    return View(eventDetail);
	    }

		[HttpPost]
		public ActionResult Register(int id, bool doRegister, string payOption, string payOrSetOption)
	    {
			try
			{
				var eventDetail = UnitOfWork.EventRepository.GetById(id);
				var price = 0;
				var bonuspoints = 0;

				if (payOption == "pay-or-set")
				{
					bonuspoints = eventDetail.Bonuspoints;
					//if (payOrSetOption == "set")
					//{
					//	bonuspoints = eventDetail.Bonuspoints;
					//}
					//else
					//{
					//	price = eventDetail.Price;
					//}
				}

				if (payOption == "pay")
				{
					price = eventDetail.Price;
				}

				if (UnitOfWork.EventRepository.AddRegistration(id, LoggedUser.Id, price, bonuspoints))
					FormSuccess = new FormFeedback();
				else
					FormError = new FormFeedback() { Text = "Die maximale Teilnehmeranzahl ist leider schon erreicht." };

				return View(UnitOfWork.EventRepository.GetById(id));
			}
			catch (Exception ex)
			{
				FormError = new FormFeedback() { Text = "Ein unerwarteter Fehler ist aufgetreten. Bitte versuche es erneut." };
				return View(UnitOfWork.EventRepository.GetById(id));
			}
	    }

		[AllowAnonymous]
		public JsonResult GetOpenSeats(int eventId)
	    {
		    return Json(UnitOfWork.EventRepository.GetOpenSeats(eventId), JsonRequestBehavior.AllowGet);
	    }

	    public JsonResult CancelRegistration(int eventId)
	    {
		    return Json(UnitOfWork.EventRepository.CancelRegistration(eventId, LoggedUser.Id), JsonRequestBehavior.AllowGet);
	    }

	    public ActionResult FeedbackForm(int id)
	    {
			var feedback =
				UnitOfWork.EventFeedbackRepository.AsQueryable()
					.FirstOrDefault(f => f.EventId == id && f.UserId == LoggedUser.Id);

		    if (feedback != null)
		    {
				FormError = new FormFeedback()
				{
					IsError = true,
					Text = "Du hast uns zu diesem Workshop schon Feedback gegeben."
				};
		    }

			return View(UnitOfWork.EventRepository.GetById(id));
	    }

		[HttpPost]
		public ActionResult FeedbackForm(int eventId, int liked, int helpful, bool recommendWorthy, string comments)
		{
			var feedback =
				UnitOfWork.EventFeedbackRepository.AsQueryable()
					.FirstOrDefault(f => f.EventId == eventId && f.UserId == LoggedUser.Id);

			if (feedback == null)
			{
				feedback = new EventFeedback()
				{
					UserId = LoggedUser.Id,
					EventId = eventId,
					Liked = liked,
					Helpful = helpful,
					RecommendWorthy = recommendWorthy,
					Comments = comments,
					CreatedDate = DateTime.Now
				};
				UnitOfWork.EventFeedbackRepository.Insert(feedback);
				UnitOfWork.Save();
				FormSuccess = new FormFeedback();
			}
			else
			{
				FormError = new FormFeedback()
				{
					IsError = true,
					Text = "Du hast uns zu diesem Workshop schon Feedback gegeben."
				};
			}

			return View(UnitOfWork.EventRepository.GetById(eventId));
		}

	    public ActionResult AttendanceForm(int id)
	    {
		    var reg = UnitOfWork.EventRegistrationRepository.GetById(id);
		    if (!string.IsNullOrEmpty(reg.Comments))
		    {
				FormError = new FormFeedback()
				{
					IsError = true,
					Text = "Du hast uns zu diesem Workshop schon Feedback gegeben."
				};
		    }
		    
			ViewBag.Id = id;
		    return View(UnitOfWork.EventRepository.GetById(reg.EventId));
	    }

		[HttpPost]
		public ActionResult AttendanceForm(int id, string comments)
		{
			UnitOfWork.EventRegistrationRepository.AddComment(id, comments);
			UnitOfWork.Save();
			FormSuccess = new FormFeedback();
			return View();
		}
    }
}
