using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class TestController : BaseController
    {
        public ActionResult Register(string email, string password, string firstname, string surname, int schoolId)
        {
	        UnitOfWork.StudentRepository.Register(email, password, firstname, surname, schoolId);
            return View("Empty");
        }

	    public ActionResult RegisterConfirm(string registerCode)
	    {
		    return View("Empty");
	    }

	    public ActionResult PasswordReset(string email)
	    {
		    return View("Empty");
	    }
    }
}
