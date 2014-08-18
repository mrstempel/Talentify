using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KwIt.Project.Pattern.DAL.Context;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;
using WebMatrix.WebData;

namespace Talentify.Web.Controllers
{
	[AllowAnonymous]
    public class RegisterController : BaseController
    {
        public ActionResult Index()
        {
            return View(new Student());
        }

		[HttpPost]
	    public ActionResult Index(Student student)
		{
			var saveFeedback = UnitOfWork.StudentRepository.Register(student, Request["token"]);

			if (saveFeedback.IsError)
				this.FormError = saveFeedback;
			else
				this.FormSuccess = saveFeedback;

			return View(student);
	    }

	    public ActionResult Confirm(string c, string t)
	    {
		    try
		    {
			    var user = UnitOfWork.StudentRepository.RegisterConfirm(new Guid(c), t);
				if (user != null && WebSecurity.Login(user.Email, user.Password))
				{
					Session["IsFirstLogin"] = true;
					return RedirectToAction("Index", "Start");
				}
		    }
		    catch (Exception ex) { }

			this.FormError = new FormFeedback();
			return View(new Login());
	    }

		public ActionResult Agb()
		{
			return View();
		}

		public ActionResult Teacher()
		{
			return View(new Teacher());
		}

		[HttpPost]
		public ActionResult Teacher(Teacher teacher)
		{
			var saveFeedback = UnitOfWork.TeacherRepository.Register(teacher, Request["CoachingSubjects"]);

			if (saveFeedback.IsError)
				this.FormError = saveFeedback;
			else
				this.FormSuccess = saveFeedback;
			return View(teacher);
		}
    }
}
