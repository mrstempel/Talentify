using System.Collections.Generic;
using System.Web.Mvc;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
	[AllowAnonymous]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
			ViewBag.AllCoachingSubjects = new SelectList(UnitOfWork.SubjectCategoryRepository.Get(), "Id", "Name");
			ViewBag.AllSchools = new SelectList(UnitOfWork.SchoolRepository.Get(), "Id", "Name");
			var classes = new List<KeyValuePair<int, string>>();
			for (int i = 1; i < 9; i++)
				classes.Add(new KeyValuePair<int, string>(i, i + ". Schulstufe"));
			ViewBag.AllClasses = new SelectList(classes, "key", "value");
            return View();
        }
    }
}
