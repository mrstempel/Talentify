﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.DAL.UnitOfWork;

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
				if (HttpContext.Current.Session["CmsContext.User"] == null)
				{
					HttpContext.Current.Session["CmsContext.User"] =
						unitOfWork.BaseUseRepository.GetByEmail(HttpContext.Current.User.Identity.Name) ?? new BaseUser();
				}
				return (BaseUser)HttpContext.Current.Session["CmsContext.User"];
			}
			set
			{
				HttpContext.Current.Session["CmsContext.User"] = value;
			}
		}

		public WebContext(TalentifyUnitOfWork<TalentifyContext> unitOfWork)
		{
			this.UnitOfWork = unitOfWork;
		}
	}
}