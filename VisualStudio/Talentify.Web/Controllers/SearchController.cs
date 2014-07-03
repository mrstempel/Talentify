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
		    return View(UnitOfWork.CoachingOfferRepository.Search(searchParams));
	    }
    }
}
