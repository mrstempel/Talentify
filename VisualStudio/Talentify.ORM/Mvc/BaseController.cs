using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.DAL.UnitOfWork;
using Talentify.ORM.FrontendLogic.Models;

namespace Talentify.ORM.Mvc
{
	[Authorize]
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
