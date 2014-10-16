using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;
using Talentify.ORM.Mvc.Security;

namespace Talentify.Web.Controllers
{
	[RequireActiveSchool]
    public class TeacherController : BaseController
    {
        public ActionResult Index()
        {
	        var student = UnitOfWork.StudentRepository.GetById(LoggedUser.Id);
            return View(new SearchParams() { SchoolId = (student.SchoolId.HasValue) ? student.SchoolId.Value : 0 });
        }

		public ActionResult Search(SearchParams searchParams)
		{
			searchParams.SearchBy = LoggedUser;
			var results = UnitOfWork.TeacherRepository.Search(searchParams);
			return View(results);
		}
    }
}
