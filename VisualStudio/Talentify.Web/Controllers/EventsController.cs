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
	        return View(UnitOfWork.EventRepository.GetOnlineEvents());
        }

	    public ActionResult Detail(int id)
	    {
		    ViewBag.DisableRegistration = UnitOfWork.EventRepository.IsUserRegistered(id, LoggedUser.Id);
		    return View(UnitOfWork.EventRepository.GetById(id));
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
    }
}
