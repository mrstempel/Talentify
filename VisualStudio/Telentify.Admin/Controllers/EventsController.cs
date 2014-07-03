using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
		public ActionResult Create(Event e, HttpPostedFileBase uploadPreview, HttpPostedFileBase uploadImage)
		{
			UnitOfWork.EventRepository.Insert(e, uploadPreview, uploadImage, Server.MapPath("~" + ConfigurationManager.AppSettings["Upload.BasePage"]));
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
		public ActionResult Edit(Event e, HttpPostedFileBase uploadPreview, HttpPostedFileBase uploadImage)
		{
			UnitOfWork.EventRepository.Update(e, uploadPreview, uploadImage, Server.MapPath("~" + ConfigurationManager.AppSettings["Upload.BasePage"]));
			UnitOfWork.Save();
			return RedirectToAction("Index");
		}
    }
}
