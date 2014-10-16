using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Talentify.ORM.Mvc.Security
{
	public class RequireActiveSchool : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (filterContext.HttpContext.Session == null || 
				(filterContext.HttpContext.Session["WebContext.HasSchool"] == null || 
				(bool)filterContext.HttpContext.Session["WebContext.HasSchool"] == false))
			{
				var res = filterContext.HttpContext.Response;
				res.Redirect("/Start");

				//filterContext.Result = QueezResultError.Error("no-session-cookie-found");
			}
		}
	}
}
