using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class NotificationController : BaseController
    {
		[AllowAnonymous]
	    public JsonResult Count()
	    {
			var count = (this.IsAuthenticated) ? UnitOfWork.NotificationRepository.Count(LoggedUser.Id) : 0;
		    return Json(count, JsonRequestBehavior.AllowGet);
	    }

		[AllowAnonymous]
		public ActionResult PopupList()
		{
			var list = (this.IsAuthenticated) ? UnitOfWork.NotificationRepository.GetPopupList(LoggedUser.Id) : null;
		    return View(list);
	    }

	    public ActionResult AllNotifications()
	    {
		    return View(UnitOfWork.NotificationRepository.GetAll(LoggedUser.Id));
	    }
    }
}
