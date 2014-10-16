using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.DAL.UnitOfWork;
using Talentify.ORM.FrontendLogic.Models;

namespace Talentify.ORM.Mvc
{
	public class WebContext
	{
		private HttpContext context = HttpContext.Current;
		private TalentifyUnitOfWork<TalentifyContext> unitOfWork;
		public TalentifyUnitOfWork<TalentifyContext> UnitOfWork
		{
			get { return unitOfWork ?? (unitOfWork = new TalentifyUnitOfWork<TalentifyContext>()); }
			set { unitOfWork = value; }
		}

		public T GetFromContext<T>(string key)
		{
			return (T)this.context.Session[key];
		}

		public void SetInContext(string key, object value)
		{
			this.context.Session[key] = value;
		}

		public BaseUser User
		{
			get
			{
				if (HttpContext.Current.Session["WebContext.User"] == null)
				{
					HttpContext.Current.Session["WebContext.User"] =
						unitOfWork.BaseUserRepository.GetByEmail(HttpContext.Current.User.Identity.Name) ?? new BaseUser();
				}
				return (BaseUser)HttpContext.Current.Session["WebContext.User"];
			}
			set
			{
				HttpContext.Current.Session["WebContext.User"] = value;
			}
		}

		public bool HasSchool
		{
			get
			{
				if (HttpContext.Current.Session["WebContext.HasSchool"] == null)
				{
					var student = unitOfWork.StudentRepository.GetById(this.User.Id);
					HttpContext.Current.Session["WebContext.HasSchool"] = (student != null && student.HasSchool);
				}
				return (bool)HttpContext.Current.Session["WebContext.HasSchool"];
			}
			set
			{
				HttpContext.Current.Session["WebContext.HasSchool"] = value;
			}
		}

		public SearchSession SearchSession
		{
			get
			{
				return (HttpContext.Current.Session["WebContext.SearchSession"] != null)
					? HttpContext.Current.Session["WebContext.SearchSession"] as SearchSession
					: null;
			}
			set { HttpContext.Current.Session["WebContext.SearchSession"] = value; }
		}

		public void ClearContext()
		{
			this.User = null;
			this.SearchSession = null;
			HttpContext.Current.Session["WebContext.HasSchool"] = null;
		}

		public WebContext(TalentifyUnitOfWork<TalentifyContext> unitOfWork)
		{
			this.UnitOfWork = unitOfWork;
		}
	}
}
