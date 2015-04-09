using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.Mvc;
using Talentify.ORM.Utils;
using Telentify.Admin.Models;

namespace Telentify.Admin.Controllers
{
    public class UserController : AdminController
    {
        public ActionResult Index()
        {
	        ViewBag.FullCount = UnitOfWork.StudentRepository.GetFullRegisteredCount();
	        ViewBag.NotConfirmed = UnitOfWork.StudentRepository.GetNotRegisterConfirmedCount();
			ViewBag.AusweisCount = UnitOfWork.StudentRepository.GetSchuelerausweisCount();
	        ViewBag.CoachCount = UnitOfWork.StudentRepository.GetCoachCount();
	        ViewBag.DeletedCount = UnitOfWork.StudentRepository.GetDeletedCount();
            return View();
        }

	    public ActionResult ConfirmedList()
	    {
		    return View(UnitOfWork.StudentRepository.GetFullRegisteredList());
	    }

		[HttpPost]
		public ActionResult Update(string[] ids, string[] blockedReasons, bool[] isWorkshopBlocked)
		{
			var users = UnitOfWork.BaseUserRepository.Get().ToList();

			for (int i = 0; i < ids.Count(); i++)
			{
				var student = users.FirstOrDefault(s => s.Id == Convert.ToInt32(ids[i]));
				if (student != null)
				{
					var prevIsActive = student.IsActive;
					student.IsActive = (blockedReasons != null && string.IsNullOrEmpty(blockedReasons[i]));
					student.BlockedReason = (blockedReasons != null) ? blockedReasons[i] : null;
					student.IsWorkshopBlocked = (isWorkshopBlocked != null) && isWorkshopBlocked[i];
					UnitOfWork.BaseUserRepository.Update(student);

					if (!student.IsActive && prevIsActive)
					{
						Email.SendBlocked(student.Email, "Dein talentify.me Account wurde deaktiviert", student.BlockedReason);
						//Email.SendBlocked("dstempel@gmail.com", "Dein talentify.me Account wurde deaktiviert", student.BlockedReason);
					}
				}
			}

			UnitOfWork.Save();
			return RedirectToAction("ConfirmedList");
		}

		public ActionResult NotConfirmedList()
		{
			return View(UnitOfWork.StudentRepository.GetNotRegisterConfirmedList());
		}

		public ActionResult AusweisList()
		{
			var users = UnitOfWork.StudentRepository.GetAusweisList();
			foreach (var u in users)
			{
				if (u.SchoolId.HasValue)
					u.School = UnitOfWork.SchoolRepository.GetById(u.SchoolId.Value);
			}
			return View(users);
		}

		[HttpPost]
	    public ActionResult AusweisList(string[] ids, bool[] unblock)
		{
			var users = UnitOfWork.BaseUserRepository.Get().ToList();

			for (int i = 0; i < ids.Count(); i++)
			{
				var student = users.FirstOrDefault(s => s.Id == Convert.ToInt32(ids[i]));
				if (student != null)
				{
					if (unblock != null && unblock[i])
					{
						student.BlockedReason = null;
						UnitOfWork.BaseUserRepository.Update(student);
						Email.SendAccountConfirmed(student.Email, ConfigurationManager.AppSettings["Email.Notifiction.Subject"]);
					}
				}
			}

			UnitOfWork.Save();

			return RedirectToAction("AusweisList");
		}

		public ActionResult CoachList()
		{
			var studentsWithOffers = new List<StudentWithCoachingOffers>();
			var students = UnitOfWork.StudentRepository.GetCoachList();
			foreach (var s in students)
			{
				var offers = UnitOfWork.CoachingOfferRepository.AsQueryable().Where(o => o.UserId == s.Id);
				var studentWithOffer = new StudentWithCoachingOffers() {Student = s, CoachingOffers = offers};
				studentsWithOffers.Add(studentWithOffer);
			}

			return View(studentsWithOffers);
		}
    }
}
