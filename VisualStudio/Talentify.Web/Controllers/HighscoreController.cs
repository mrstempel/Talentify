using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Talentecheck;
using Talentify.ORM.DAL.Repository;
using Talentify.ORM.Mvc;
using Talentify.Web.Models.Talentecheck;

namespace Talentify.Web.Controllers
{
    public class HighscoreController : BaseController
    {
        public ActionResult Talentecheck()
        {
	        if (Request["s"] != null && Convert.ToBoolean(Request["s"]))
	        {
		        // if talentecheck register, save user-id to talentecheck-session
		        if (this.TalentecheckSessionFromCookie != null && this.TalentecheckSession == null)
		        {
			        var badgeUser = UnitOfWork.BaseUserRepository.GetById(LoggedUser.Id);
			        this.TalentecheckSessionFromCookie.UserId = LoggedUser.Id;
			        UnitOfWork.TalentecheckSessionRepository.Update(this.TalentecheckSessionFromCookie);

			        var talentecheckBonus = new TalentecheckBonus()
			        {
				        Action = TalentecheckBonusAction.Register.ToString(),
				        Points = TalentecheckBonusPointsFor.Register,
				        CreateDate = DateTime.Now,
				        TalentecheckSessionId = TalentecheckSessionFromCookie.Id
			        };
			        UnitOfWork.TalentecheckBonusRepository.Insert(talentecheckBonus);
					UnitOfWork.BadgeRepository.AddBadgeToUser(badgeUser, TalentecheckSessionFromCookie.TypMax.ToString());
			        UnitOfWork.Save();

			        if (Request.Cookies["TalentecheckGuid"] != null)
			        {
				        var myCookie = new HttpCookie("TalentecheckGuid");
				        myCookie.Expires = DateTime.Now.AddDays(-1);
				        Response.Cookies.Add(myCookie);
			        }
		        }
	        }

	        var myscore =
		        UnitOfWork.TalentecheckHighscoreRepository.AsQueryable()
			        .FirstOrDefault(h => h.TalentecheckSessionId == TalentecheckSession.Id);
	        ViewBag.MyHighscore = (myscore != null) ? myscore.Points : 0;

			var highscores = (from h in UnitOfWork.TalentecheckHighscoreRepository.AsQueryable()
							  join
								  tc in UnitOfWork.TalentecheckSessionRepository.AsQueryable() on h.TalentecheckSessionId equals tc.Id
							  join user in UnitOfWork.BaseUserRepository.AsQueryable() on tc.UserId equals user.Id

							  select new TalentecheckHighscoreItem()
							  {
								  Firstname = user.Firstname,
								  Surname = user.Surname,
								  Points = h.Points
							  }).OrderByDescending(i => i.Points).Take(10).ToList();

			var max = 0;
			foreach (var item in highscores)
			{
				if (max == 0)
				{
					max = item.Points;
				}

				item.Rank = Convert.ToInt16(((double)item.Points / max) * 10);
			}

	        ViewBag.HighscoreList = highscores;

            return View();
        }

		public JsonResult AddShareBonus(string network)
		{
			UnitOfWork.TalentecheckSessionRepository.AddShareBonus(network, TalentecheckSession);
			UnitOfWork.Save();
			return Json(true, JsonRequestBehavior.AllowGet);
		}

    }
}
