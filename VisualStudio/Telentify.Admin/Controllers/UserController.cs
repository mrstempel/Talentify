using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.Mvc;
using Telentify.Admin.Models;

namespace Telentify.Admin.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
	        ViewBag.FullCount = UnitOfWork.StudentRepository.GetFullRegisteredCount();
	        ViewBag.NotConfirmed = UnitOfWork.StudentRepository.GetNotRegisterConfirmedCount();
	        ViewBag.CoachCount = UnitOfWork.StudentRepository.GetCoachCount();
	        ViewBag.DeletedCount = UnitOfWork.StudentRepository.GetDeletedCount();
            return View();
        }

	    public ActionResult ConfirmedList()
	    {
		    return View(UnitOfWork.StudentRepository.GetFullRegisteredList());
	    }

		public ActionResult NotConfirmedList()
		{
			return View(UnitOfWork.StudentRepository.GetNotRegisterConfirmedList());
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
