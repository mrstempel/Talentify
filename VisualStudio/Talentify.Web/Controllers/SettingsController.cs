using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.FrontendLogic;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class SettingsController : BaseController
    {
        public ActionResult Index()
        {
            return View(new PasswordChange());
        }

		[HttpPost]
	    public ActionResult Index(PasswordChange passwordChange)
	    {
			try
			{
				if (UnitOfWork.BaseUserRepository.UpdatePassword(LoggedUser, passwordChange))
					FormSuccess = new FormFeedback() { AutoClose = true };
				else
					FormError = new FormFeedback() { Text = "Dein altes Passwort war nicht korrekt."};
			}
			catch (Exception ex)
			{
				FormError = new FormFeedback();
			}

			return Index();
	    }

		public JsonResult SetNotificationsEnabled(bool isEnabled)
		{
			try
			{
				LoggedUser.Settings.HasNotifications = isEnabled;
				UnitOfWork.UserSettingsRepository.Update(LoggedUser.Settings);
				UnitOfWork.Save();
				return Json(true, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(false, JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult SetNewsletterEnabled(bool isEnabled)
		{
			try
			{
				LoggedUser.Settings.HasNewsletter = isEnabled;
				UnitOfWork.UserSettingsRepository.Update(LoggedUser.Settings);
				UnitOfWork.Save();

				if (isEnabled)
				{
					var student = UnitOfWork.StudentRepository.GetById(LoggedUser.Id);
					NewsletterRegistration.Subscribe(student);
				}
				else
				{
					NewsletterRegistration.Unsubscribe(LoggedUser.Email);
				}

				return Json(true, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(false, JsonRequestBehavior.AllowGet);
			}
		}
    }
}
