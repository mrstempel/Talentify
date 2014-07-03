using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;

namespace Talentify.ORM.Mvc
{
	public class BaseView<T> : WebViewPage<T>
	{
		public bool IsAuthenticated
		{
			get { return HttpContext.Current.User.Identity.IsAuthenticated; }
		}

		public BaseUser LoggedUser
		{
			get { return BaseController.LoggedUser; }
		}

		private BaseController _baseController;
		/// <summary>
		/// Reference for base controller
		/// </summary>
		protected virtual BaseController BaseController
		{
			get
			{
				if (_baseController == null)
				{
					_baseController = ((BaseController)this.ViewContext.Controller);
				}
				return _baseController;
			}
		}

		public FormFeedback FormError
		{
			get { return this.BaseController.FormError; }
		}

		public FormFeedback FormSuccess
		{
			get { return this.BaseController.FormSuccess; }
		}

		public bool IsFirstLogin
		{
			get { return (ViewBag.IsFirstLogin != null && Convert.ToBoolean(ViewBag.IsFirstLogin)); }
		}

		public string ContentUploadUrl
		{
			get { return string.Format("{0}/pages/", ConfigurationManager.AppSettings["Admin.Upload.Url"]); }
		}

		public override void Execute()
		{
		}
	}
}
