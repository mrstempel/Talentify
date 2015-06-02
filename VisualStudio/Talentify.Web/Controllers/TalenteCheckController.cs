using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Talentecheck;
using Talentify.ORM.Mvc;
using Talentify.Web.Models.Talentecheck;

namespace Talentify.Web.Controllers
{
	public class TalenteCheckController : TalentecheckBaseController
    {
		//[AllowAnonymous]
		//public ActionResult Index()
		//{
		//	if (!string.IsNullOrEmpty(Request["s"]))
		//	{
		//		Guid inviteGuid;
		//		if (Guid.TryParse(Request["s"], out inviteGuid))
		//		{
		//			Session["TalentecheckInviteGuid"] = inviteGuid;
		//		}
		//	}


		//	return View();
		//}

		//[AllowAnonymous]
		//public ActionResult Umfrage(int f = 1)
		//{
		//	ViewBag.Frage = f;
		//	return View();
		//}

		//[AllowAnonymous]
		//public ActionResult Frage(int f = 1)
		//{
		//	ViewBag.Frage = f;
		//	Type t = TalentecheckSession.GetType();
		//	ViewBag.Answer = Convert.ToInt16(t.GetProperty("Frage" + f).GetValue(TalentecheckSession));
		//	return View();
		//}

		//[AllowAnonymous]
		//public JsonResult SetAnswer(int f, int a)
		//{
		//	if (UnitOfWork.TalentecheckSessionRepository.SetAnswer(TalentecheckSession, f, a))
		//	{
		//		if (f == 10)
		//		{
		//			// set finished
		//			UnitOfWork.TalentecheckSessionRepository.SetFinished(TalentecheckSession);
		//		}

		//		UnitOfWork.Save();
		//		return Json(true, JsonRequestBehavior.AllowGet);
		//	}

		//	return Json(false, JsonRequestBehavior.AllowGet);
		//}

		//[AllowAnonymous]
		//public ActionResult Result()
		//{
		//	if (!TalentecheckSession.IsFinished)
		//	{
		//		return RedirectToAction("Index");
		//	}

		//	var resultPageData = new ResultPageData();

		//	resultPageData.PlusType = TalentecheckSession.TypMax.ToString();
		//	resultPageData.PlusTypeReadable = UnitOfWork.TalentecheckSessionRepository.GetTypReadable(TalentecheckSession.TypMax);
		//	resultPageData.PlusPercent = TalentecheckSession.MaxPercent;
		//	resultPageData.MinusType = TalentecheckSession.TypMin.ToString();
		//	resultPageData.MinusTypeReadable = UnitOfWork.TalentecheckSessionRepository.GetTypReadable(TalentecheckSession.TypMin);
		//	resultPageData.MinusPercent = TalentecheckSession.MinPercent;
		//	resultPageData.Sex = TalentecheckSession.Frage10;

		//	// was können die anderen
		//	resultPageData.TypeOverview = UnitOfWork.TalentecheckSessionRepository.GetTypeOverview();

		//	var highscores = (from h in UnitOfWork.TalentecheckHighscoreRepository.AsQueryable()
		//		join
		//			tc in UnitOfWork.TalentecheckSessionRepository.AsQueryable() on h.TalentecheckSessionId equals tc.Id
		//		join user in UnitOfWork.BaseUserRepository.AsQueryable() on tc.UserId equals user.Id

		//		select new TalentecheckHighscoreItem()
		//		{
		//			Firstname = user.Firstname,
		//			Surname = user.Surname,
		//			Points = h.Points
		//		}).OrderByDescending(i => i.Points).Take(10).ToList();

		//	var max = 0;
		//	foreach (var item in highscores)
		//	{
		//		if (max == 0)
		//		{
		//			max = item.Points;
		//		}

		//		item.Rank = Convert.ToInt16(((double)item.Points/max)*10);
		//	}

		//	resultPageData.HighscoreItems = highscores;

		//	return View(resultPageData);
		//}

		//public JsonResult AddShareBonus(string network)
		//{
		//	UnitOfWork.TalentecheckSessionRepository.AddShareBonus(network, TalentecheckSession);
		//	UnitOfWork.Save();
		//	return Json(true, JsonRequestBehavior.AllowGet);
		//}
    }
}
