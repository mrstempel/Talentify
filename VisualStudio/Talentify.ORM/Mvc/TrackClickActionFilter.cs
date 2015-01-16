using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Tracking;

namespace Talentify.ORM.Mvc
{
	public class TrackClickActionFilter : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			try
			{
				var baseController = filterContext.Controller as BaseController;
				var clickTracking = new TrackingClick()
				{
					UserId = baseController.LoggedUser.Id,
					InsertDate = DateTime.Now,
					Link = filterContext.HttpContext.Request.Url.ToString()
				};
				baseController.UnitOfWork.TrackingClickRepository.Insert(clickTracking);
				baseController.UnitOfWork.Save();
			}
			catch (Exception) { }
		}
	}
}
