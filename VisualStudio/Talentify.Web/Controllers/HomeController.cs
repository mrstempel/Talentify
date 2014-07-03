using System.Web.Mvc;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
	[AllowAnonymous]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
