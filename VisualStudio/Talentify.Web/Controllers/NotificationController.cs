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
	    public JsonResult Count()
	    {
			var count = UnitOfWork.NotificationRepository.Count(LoggedUser.Id);
		    return Json(count, JsonRequestBehavior.AllowGet);
	    }

		public ActionResult PopupList()
		{
			var list = UnitOfWork.NotificationRepository.GetPopupList(LoggedUser.Id);
		    return View(list);
	    }

	    public ActionResult AllNotifications()
	    {
		    return View(UnitOfWork.NotificationRepository.GetAll(LoggedUser.Id));
	    }
    }
}
