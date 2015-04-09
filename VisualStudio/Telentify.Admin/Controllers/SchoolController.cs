using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Content;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.Mvc;

namespace Telentify.Admin.Controllers
{
	public class SchoolController : AdminController
    {
		private SelectList _allSchoolTypes;
		public SelectList AllSchoolTypes
		{
			get
			{
				if (_allSchoolTypes == null)
				{
					_allSchoolTypes = new SelectList(UnitOfWork.SchoolTypeRepository.Get(), "Id", "Code");
				}

				return _allSchoolTypes;
			}
		}

        public ActionResult Index()
        {
			var allSchholTypes = UnitOfWork.SchoolTypeRepository.Get().ToList();
			allSchholTypes.Insert(0, new SchoolType() { Id = 0, Code = "Schultyp" });
			ViewBag.AllSchoolTypes = allSchholTypes;
            return View(UnitOfWork.SchoolRepository.Get());
        }

		[HttpPost]
		public ActionResult Index(string bundesland, int schoolTypeId, string name, string address)
		{
			var allSchholTypes = UnitOfWork.SchoolTypeRepository.Get().ToList();
			allSchholTypes.Insert(0, new SchoolType() { Id = 0, Code = "Schultyp" });
			ViewBag.AllSchoolTypes = allSchholTypes;
			return View(UnitOfWork.SchoolRepository.SearchSchools(bundesland, schoolTypeId, name, address, false));
		}

	    public ActionResult Create()
	    {
			ViewBag.AllSchoolTypes = AllSchoolTypes;
		    return View(new School());
	    }

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Create(School school)
		{
			UnitOfWork.SchoolRepository.Insert(school);
			UnitOfWork.Save();
			return RedirectToAction("Index");
		}

	    public ActionResult Edit(int id)
	    {
			ViewBag.AllSchoolTypes = AllSchoolTypes;
		    var school = UnitOfWork.SchoolRepository.GetById(id);
			return View(school);
	    }

		[HttpPost]
		[ValidateInput(false)]
		public ActionResult Edit(School school)
		{
			UnitOfWork.SchoolRepository.Update(school);
			UnitOfWork.Save();
			return RedirectToAction("Index");
		}

		public ActionResult CodeVerwaltung(int id)
		{
			var school = UnitOfWork.SchoolRepository.GetById(id);
			ViewBag.School = school;
			var codes = UnitOfWork.RegisterCodeRepository.Get(c => c.SchoolId == id);
			return View("CodeVerwaltung", codes);
		}

		public ActionResult CreateNewCodes(int id)
		{
			UnitOfWork.RegisterCodeRepository.GenerateNewCodes(id);
			UnitOfWork.Save();
			return CodeVerwaltung(id);
		}
    }
}
