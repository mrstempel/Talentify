using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class EventsController : BaseController
    {
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Filter(string filter, bool isFilter)
		{
			ViewBag.RegisteredIds = UnitOfWork.EventRepository.GetUserRegisteredEventIds(LoggedUser.Id);
			return View(UnitOfWork.EventRepository.Filter(filter, LoggedUser.Id));
		}

		[AllowAnonymous]
	    public ActionResult Detail(int id)
	    {
			ViewBag.IsUserRegistered = UnitOfWork.EventRepository.IsUserRegistered(id, LoggedUser.Id);
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

			if (eventDetail != null)
				ViewBag.IsUserRegistered = UnitOfWork.EventRepository.IsUserRegistered(eventDetail.Id, LoggedUser.Id);

			return View("Detail", eventDetail);
		}

	    public ActionResult Register(int id)
	    {
		    return View(UnitOfWork.EventRepository.GetById(id));
	    }

		[HttpPost]
	    public ActionResult Register(int id, bool doRegister)
	    {
			try
			{
				if (UnitOfWork.EventRepository.AddRegistration(id, LoggedUser.Id))
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
    }
}
