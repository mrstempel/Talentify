using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

		public bool HasSchool
		{
			get { return BaseController.WebContext.HasSchool; }
		}

		public bool IsCoachingEnabled
		{
			get { return BaseController.WebContext.IsCoachingEnabled; }
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
			get
			{
				//return true;
				return (ViewBag.IsFirstLogin != null && Convert.ToBoolean(ViewBag.IsFirstLogin));
			}
		}

		public string ContentUploadUrl
		{
			get { return string.Format("{0}/pages/", ConfigurationManager.AppSettings["Admin.Upload.Url"]); }
		}

		public bool HasSearchSession
		{
			get { return BaseController.HasSearchSession;  }
		}
		public SearchSession SearchSession
		{
			get { return BaseController.SearchSession; }
		}

		public int NotificationCount
		{
			get { return BaseController.UnitOfWork.NotificationRepository.Count(LoggedUser.Id); }
		}

		public string BaseUrl
		{
			get { return ConfigurationManager.AppSettings["BaseUrl"]; }
		}

		private ActionToken inviteToken;
		public ActionToken InviteToken
		{
			get
			{
				if (inviteToken == null)
				{
					inviteToken = BaseController.UnitOfWork.ActionTokenRepository.GetInviteToken(LoggedUser.Id);
				}

				return inviteToken;
			}
		}

		public override void Execute()
		{
		}

		public string StripHtmlTags(string text)
		{
			var noHtml = Regex.Replace(text, @"<[^>]+>|&nbsp;", "").Trim();
			var noHtmlNormalised = Regex.Replace(noHtml, @"\s{2,}", " ");

			return noHtmlNormalised;
		}
	}
}
