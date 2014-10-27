using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.Mvc;

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
			return View(UnitOfWork.StudentRepository.GetCoachList());
		}
    }
}
