using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.Mvc;

namespace Telentify.Admin.Controllers
{
    public class SubjectController : AdminController
    {
        public ActionResult Index()
        {
            return View("Index", UnitOfWork.SubjectCategoryRepository.Get().OrderBy(s => s.Sorter));
        }

		[HttpPost]
		public ActionResult Update(string[] ids, string[] isActive, string[] sorter)
		{
			var allSubjects = UnitOfWork.SubjectCategoryRepository.Get();

			for (int i = 0; i < ids.Count(); i++)
			{
				var subject = allSubjects.FirstOrDefault(s => s.Id == Convert.ToInt32(ids[i]));
				if (subject != null)
				{
					subject.Sorter = Convert.ToInt32(sorter[i]);
					subject.IsActive = isActive.Contains(ids[i]);
					UnitOfWork.SubjectCategoryRepository.Update(subject);
				}
			}
			UnitOfWork.Save();
		    return RedirectToAction("Index");
	    }

		[HttpPost]
		public ActionResult Create(string name, int sorter)
		{
			if (!string.IsNullOrEmpty(name))
			{
				UnitOfWork.SubjectCategoryRepository.Insert(new SubjectCategory() {Name = name, Sorter = sorter});
				UnitOfWork.Save();
			}

			return RedirectToAction("Index");
		}
    }
}
