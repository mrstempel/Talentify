using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Content;
using Talentify.ORM.Mvc;

namespace Telentify.Admin.Controllers
{
    public class EventsController : BaseController
    {
        public ActionResult Index()
        {
            return View(UnitOfWork.EventRepository.Get());
        }

	    public ActionResult Create()
	    {
		    return View(new Event() { CreatedById =  LoggedUser.Id, IsOnline = true });
	    }

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(Event e, HttpPostedFileBase uploadPreview, HttpPostedFileBase uploadImage, HttpPostedFileBase homeImage)
		{
			UnitOfWork.EventRepository.Insert(e, uploadPreview, uploadImage, homeImage, Server.MapPath("~" + ConfigurationManager.AppSettings["Upload.BasePage"]));
			UnitOfWork.Save();
			return RedirectToAction("Index");
		}

	    public ActionResult Edit(int id)
	    {
		    var e = UnitOfWork.EventRepository.GetById(id);
		    e.UpdateById = LoggedUser.Id;
		    return View(e);
	    }

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Edit(Event e, HttpPostedFileBase uploadPreview, HttpPostedFileBase uploadImage, HttpPostedFileBase homeImage)
		{
			UnitOfWork.EventRepository.Update(e, uploadPreview, uploadImage, homeImage, Server.MapPath("~" + ConfigurationManager.AppSettings["Upload.BasePage"]));
			UnitOfWork.Save();
			return RedirectToAction("Index");
		}

	    public ActionResult Registrations(int id)
	    {
			var e = UnitOfWork.EventRepository.GetById(id);
			ViewBag.EventTitle = e.Title;
		    ViewBag.Id = e.Id;
		    var registrations = UnitOfWork.EventRepository.GetRegistrations(id);
			return View(registrations);
	    }

		[HttpPost]
		public ActionResult Registrations(int id, string[] confirmed)
		{
			var e = UnitOfWork.EventRepository.GetById(id);
			ViewBag.EventTitle = e.Title;
			ViewBag.Id = e.Id;
			var registrations = UnitOfWork.EventRepository.GetRegistrations(id);
			List<int> sendBonus = new List<int>();

			foreach (var reg in registrations)
			{
				var isConfirmed = (confirmed != null && confirmed.Contains(reg.Id.ToString()));
				if (isConfirmed && !reg.Confirmed)
				{
					sendBonus.Add(reg.Id);
				}
				reg.Confirmed = isConfirmed;
				UnitOfWork.EventRegistrationRepository.Update(reg);
			}

			foreach (var reg in registrations)
			{
				if (sendBonus.Contains(reg.Id))
				{
					UnitOfWork.BonuspointRepository.Insert(reg.UserId, BonusPointsFor.EventConfirm, "An Event teilgenommen", 0, false);
				}
			}

			UnitOfWork.Save();

			return View(registrations);
		}
    }
}
