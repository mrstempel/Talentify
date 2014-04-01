using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.DAL.UnitOfWork;

namespace Talentify.ORM.Mvc
{
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

		public WebContext WebContext { get; set; }

		public BaseController()
		{
			WebContext = new WebContext(this.UnitOfWork);
		}
	}
}
