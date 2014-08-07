using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class InviteController : BaseController
    {
        public ActionResult Index()
        {
            return View(UnitOfWork.ActionTokenRepository.GetInviteToken(LoggedUser.Id));
        }

		[HttpPost]
		public ActionResult Index(string email)
		{
			var inviteToken = UnitOfWork.ActionTokenRepository.GetInviteToken(LoggedUser.Id);
			var formFeedback = UnitOfWork.BaseUserRepository.SenInvite(inviteToken, email);
			if (formFeedback.IsError)
			{
				FormError = formFeedback;
			}
			else
			{
				FormSuccess = formFeedback;
			}
		    return View(inviteToken);
	    }
    }
}
