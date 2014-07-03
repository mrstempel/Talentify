using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers.Api
{
	[AllowAnonymous]
    public class AutocompleteController : BaseController
    {
        public JsonResult School(string term)
        {
	        var schools = UnitOfWork.SchoolRepository.AsQueryable().Where(s => s.Name.Contains(term));
	        var autoSchools = new List<AutocompleteItem>();

	        foreach (var school in schools)
	        {
		        autoSchools.Add(new AutocompleteItem()
		        {
			        Id =  school.Id, 
					Label = school.Name, 
					Value = school.Id.ToString(), 
					Description = school.Address + ", " + school.ZipCode + " " + school.City
		        });
	        }

			return Json(autoSchools, JsonRequestBehavior.AllowGet);
        }
    }
}
