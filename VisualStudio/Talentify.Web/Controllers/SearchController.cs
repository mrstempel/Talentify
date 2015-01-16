using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class SearchController : BaseController
    {
        public ActionResult Index(SearchParams searchParams)
        {
            return View(searchParams);
        }

	    public ActionResult Search(SearchParams searchParams)
	    {
		    searchParams.SearchBy = LoggedUser;
		    var results = UnitOfWork.CoachingOfferRepository.Search(searchParams);
			var random = new Random();
			var randomResultList = results.OrderBy(i => random.Next()).ToList();
		    searchParams.SubjectCategory = UnitOfWork.SubjectCategoryRepository.GetById(searchParams.SubjectCategoryId);
		    this.WebContext.SearchSession = new SearchSession() {SearchParams = searchParams};
			return View(randomResultList);
	    }
    }
}
