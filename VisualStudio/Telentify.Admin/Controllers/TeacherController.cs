using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.Mvc;
using Telentify.Admin.Models;

namespace Telentify.Admin.Controllers
{
	public class TeacherController : AdminController
    {
        public ActionResult Index()
        {
			return View(UnitOfWork.TeacherRepository.Get());
        }

		[HttpPost]
		public ActionResult Update(string[] ids, string[] isActive)
		{
			var allTeachers = UnitOfWork.TeacherRepository.Get();
			
			for (int i = 0; i < ids.Count(); i++)
			{
				var teacher = allTeachers.FirstOrDefault(s => s.Id == Convert.ToInt32(ids[i]));
				if (teacher != null)
				{
					teacher.IsActive = (isActive != null && isActive.Contains(ids[i]));
					UnitOfWork.TeacherRepository.Update(teacher);
				}
			}

			UnitOfWork.Save();
			return RedirectToAction("Index");
		}
    }
}
