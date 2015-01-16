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

		public ActionResult Events()
		{
			return View(UnitOfWork.EventRepository.GetOnlineEvents().Take(3));
		}

		public ActionResult MailChimpTest()
		{
			var mailchimpApiService = new MailChimpApiService(MailChimpServiceConfiguration.Settings.ApiKey);
			
			var subscribeSources = new Grouping { Name = "Subscribe Source" };
			subscribeSources.Groups.Add("Platform");

			var fields = new Dictionary<string, string>
                    {
                        {"FNAME", "Vorname"},
						{"LNAME", "Nachname"},
						{"GESCHLECHT", "Männlich"},
						{"SCHULE", "Schule"},
						{"KLASSE", "Klasse"},
						{"TELEFON", "Telefon"}
                    };

			var response = mailchimpApiService.Subscribe("dstempel@gmail.com", new List<Grouping>() { subscribeSources }, fields, false);
			ViewBag.Success = response.IsSuccesful;
			ViewBag.Mailchimp = response.ResponseJson;
			return View();
		}

		public ActionResult MailChimpTest2()
		{
			var mailchimpApiService = new MailChimpApiService(MailChimpServiceConfiguration.Settings.ApiKey);

			var response = mailchimpApiService.Unsubscribe("dstempel@gmail.com");
			ViewBag.Success = response.IsSuccesful;
			ViewBag.Mailchimp = response.ResponseJson;
			return View();
		}
    }
}
