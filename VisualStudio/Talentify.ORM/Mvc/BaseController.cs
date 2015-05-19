using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Talentecheck;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.DAL.UnitOfWork;
using Talentify.ORM.FrontendLogic.Models;

namespace Talentify.ORM.Mvc
{
	[Authorize]
	[RequireHttps]
	public class BaseController : Controller
	{
		public BaseUser LoggedUser
		{
			get { return WebContext.User; }
		}

		private TalentifyUnitOfWork<TalentifyContext> unitOfWork;
		public TalentifyUnitOfWork<TalentifyContext> UnitOfWork
		{
			get
			{
				if (unitOfWork == null)
					unitOfWork = new TalentifyUnitOfWork<TalentifyContext>();


				return unitOfWork;
			}
		}

		public FormFeedback FormError { get; set; }
		public FormFeedback FormSuccess { get; set; }
		public WebContext WebContext { get; set; }

		public virtual bool KeepSearchSessionAlive { get; set; }
		public SearchSession SearchSession
		{
			get { return WebContext.SearchSession;  }
			set { WebContext.SearchSession = value; }
		}
		public bool HasSearchSession
		{
			get { return SearchSession != null; }
		}
		public bool IsAuthenticated
		{
			get { return User.Identity.IsAuthenticated; }
		}

		private TalentecheckSession talentecheckSessionFromCookie;
		public virtual TalentecheckSession TalentecheckSessionFromCookie
		{
			get
			{
				if (talentecheckSessionFromCookie == null && Request.Cookies["TalentecheckGuid"] != null)
				{
					Guid cookieGuid;
					bool isvalid = Guid.TryParse(Request.Cookies["TalentecheckGuid"].Value, out cookieGuid);
					if (isvalid)
					{
						talentecheckSessionFromCookie =
							UnitOfWork.TalentecheckSessionRepository.AsQueryable()
								.FirstOrDefault(s => s.SessionId == cookieGuid);
					}
				}

				return talentecheckSessionFromCookie;
			}
		}

		private TalentecheckSession talentecheckSession;
		public virtual TalentecheckSession TalentecheckSession
		{
			get
			{
				if (talentecheckSession == null)
				{
					talentecheckSession = UnitOfWork.BaseUserRepository.GetTalentecheckSession(LoggedUser);
				}

				return talentecheckSession;
			}
		}

		public BaseController()
		{
			WebContext = new WebContext(this.UnitOfWork);
		}

		protected override void Dispose(bool disposing)
		{
			//dispose unitofwork
			if (this.UnitOfWork != null)
				this.UnitOfWork.Dispose();

			base.Dispose(disposing);
		}
	}
}
