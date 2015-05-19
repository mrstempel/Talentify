using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using MailChimp.Net.Api;
using MailChimp.Net.Api.Domain;
using MailChimp.Net.Settings;
using Talentify.ORM.DAL.Models.Tracking;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
	[AllowAnonymous]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
			ViewBag.AllCoachingSubjects = new SelectList(UnitOfWork.SubjectCategoryRepository.Get(), "Id", "Name");
			ViewBag.AllSchools = new SelectList(UnitOfWork.SchoolRepository.Get(), "Id", "Name");
			var classes = new List<KeyValuePair<int, string>>();
			for (int i = 1; i < 9; i++)
				classes.Add(new KeyValuePair<int, string>(i, i + ". Schulstufe"));
			ViewBag.AllClasses = new SelectList(classes, "key", "value");
            return View();
        }

		public ActionResult Tour()
		{
			return View();
		}

		public ActionResult Voice()
		{
			return View();
		}

		public ActionResult About()
		{
			return View();
		}

		public ActionResult Events()
		{
			return View(UnitOfWork.EventRepository.GetOnlineEvents().Take(3));
		}
    }
}
