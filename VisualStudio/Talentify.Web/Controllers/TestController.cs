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
        //
        // GET: /Test/

        public ActionResult Index()
        {
	        var testUser = new Student()
	        {
		        Email = "dstempel@gmail.com",
		        Password = PasswordHashing.CalculateSha1("haifisch"),
		        Firstname = "David",
		        Surname = "Stempel",
		        SchoolId = 1
	        };
	        UnitOfWork.BaseUseRepository.Register(testUser);
            return View();
        }

    }
}
