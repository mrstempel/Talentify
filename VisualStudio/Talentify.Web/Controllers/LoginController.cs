using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
	[AllowAnonymous]
    public class LoginController : BaseController
    {
        public ActionResult Error()
        {
            return View();
        }

		public ActionResult Password()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Password(string email)
		{
			UnitOfWork.BaseUserRepository.StartPasswordReset(email);
			FormSuccess = new FormFeedback();
			return View();
		}

		public ActionResult PasswordReset(Guid token)
		{
			// check token
			var resetToken = UnitOfWork.ActionTokenRepository.Get(token, ActionTokenType.PasswordReset);
			ViewBag.ShowForm = resetToken != null;
			
			if (resetToken != null)
			{
			}
			else
			{
				FormError = new FormFeedback() { Headline = "Diese Aktion ist nicht erlaubt", Text = ""};
			}

			return View();
		}

		[HttpPost]
		public ActionResult PasswordReset(Guid token, string password)
		{
			var actionToken = UnitOfWork.ActionTokenRepository.Get(token, ActionTokenType.PasswordReset);
			var isResetOkay = UnitOfWork.BaseUserRepository.ResetPassword(actionToken, password);
			if (isResetOkay)
			{
				FormSuccess = new FormFeedback();
			}
			else
			{
				FormError = new FormFeedback() { Headline = "Ein unerwarteter Fehler ist aufgetreten", Text = "Bitte versuche es erneut." };
			}

			return View();
		}
    }
}
