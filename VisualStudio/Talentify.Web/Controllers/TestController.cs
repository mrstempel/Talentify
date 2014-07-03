using System.Web.Mvc;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class TestController : BaseController
    {
        public ActionResult Register(string email, string password, string firstname, string surname, int schoolId)
        {
	        UnitOfWork.StudentRepository.Register(new Student() { Email =  email, Password =  password, Firstname =  firstname, Surname =  surname, SchoolId = schoolId});
            return View("Empty");
        }

	    public ActionResult RegisterConfirm(string registerCode)
	    {
		    return View("Empty");
	    }

	    public ActionResult PasswordReset(string email)
	    {
		    return View("Empty");
	    }
    }
}
