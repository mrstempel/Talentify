using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using Talentify.ORM.DAL.Models.Talentecheck;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.DAL.Repository;
using Talentify.ORM.Mvc;
using WebMatrix.WebData;

namespace Talentify.Web.Controllers.Api
{
	[AllowAnonymous]
    public class AuthController : BaseController
    {
		public JsonResult Login(string email, string password)
		{
			var loginSuccess = 0;
			try
			{
				if (WebSecurity.Login(email, password))
				{
					// if talentecheck register, save user-id to talentecheck-session
					if (this.TalentecheckSessionFromCookie != null && this.TalentecheckSession == null)
					{
						var user = Session["WebContext.User"] as BaseUser;
						this.TalentecheckSessionFromCookie.UserId = user.Id;
						UnitOfWork.TalentecheckSessionRepository.Update(this.TalentecheckSessionFromCookie);

						var talentecheckBonus = new TalentecheckBonus()
						{
							Action = TalentecheckBonusAction.Register.ToString(),
							Points = TalentecheckBonusPointsFor.Register,
							CreateDate = DateTime.Now,
							TalentecheckSessionId = TalentecheckSessionFromCookie.Id
						};
						UnitOfWork.TalentecheckBonusRepository.Insert(talentecheckBonus);
						UnitOfWork.BadgeRepository.AddBadgeToUser(user, TalentecheckSessionFromCookie.TypMax.ToString());
						UnitOfWork.Save();

						if (Request.Cookies["TalentecheckGuid"] != null)
						{
							var myCookie = new HttpCookie("TalentecheckGuid");
							myCookie.Expires = DateTime.Now.AddDays(-1);
							Response.Cookies.Add(myCookie);
						}
						loginSuccess = 2;
					}
					else
					{
						loginSuccess = 1;	
					}
				}
			}
			catch (Exception ex) {}


			return Json(loginSuccess, JsonRequestBehavior.AllowGet);
        }

		public JsonResult Logout()
		{
			WebSecurity.Logout();
			this.WebContext.ClearContext();
			return Json(true, JsonRequestBehavior.AllowGet);
		}

		public JsonResult DeleteAccount()
		{
			UnitOfWork.StudentRepository.DeleteAccount(LoggedUser.Id, Server.MapPath("~" + ConfigurationManager.AppSettings["Upload.Profile"]));
			return Json(true, JsonRequestBehavior.AllowGet);
		}
    }
}
