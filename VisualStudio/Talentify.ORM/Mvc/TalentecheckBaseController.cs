using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Talentify.ORM.DAL.Models.Talentecheck;

namespace Talentify.ORM.Mvc
{
	public class TalentecheckBaseController : BaseController
	{
		private Guid? talentecheckGuid;
		public Guid? TalentecheckGuid
		{
			get
			{
				if (!talentecheckGuid.HasValue)
				{
					InitGuid();
				}

				return talentecheckGuid;
			}
		}

		private TalentecheckSession talentecheckSession;
		public override TalentecheckSession TalentecheckSession
		{
			get
			{
				if (talentecheckSession == null)
					talentecheckSession =
						UnitOfWork.TalentecheckSessionRepository.AsQueryable().FirstOrDefault(s => s.SessionId == TalentecheckGuid.Value);

				return talentecheckSession;
			}
		}

		private void InitGuid()
		{
			if (!talentecheckGuid.HasValue && Request.Cookies["TalentecheckGuid"] != null)
			{
				Guid cookieGuid;
				bool isvalid = Guid.TryParse(Request.Cookies["TalentecheckGuid"].Value, out cookieGuid);
				if (isvalid && Session["checkGuid"] == null)
				{
					if (UnitOfWork.TalentecheckSessionRepository.AsQueryable().Count(m => m.SessionId == cookieGuid) == 1)
						talentecheckGuid = cookieGuid;

					Session.Add("checkGuid", true);
				}
				else if (isvalid && Session["checkGuid"] != null)
				{
					talentecheckGuid = cookieGuid;
				}
			}

			if (!talentecheckGuid.HasValue)
			{
				talentecheckGuid = Guid.NewGuid();
				//save to DB
				talentecheckSession = new TalentecheckSession { SessionId = talentecheckGuid.Value, StartTime = DateTime.Now };
				if (Session["TalentecheckInviteGuid"] != null)
				{
					try
					{
						talentecheckSession.InviteSessionId = (Guid)Session["TalentecheckInviteGuid"];
						Session["TalentecheckInviteGuid"] = null;
					}
					catch (Exception ex) { }
					
				}
				UnitOfWork.TalentecheckSessionRepository.Insert(talentecheckSession);
				UnitOfWork.Save();
				//save to cookie
				var cookie = new HttpCookie("TalentecheckGuid", talentecheckGuid.ToString()) { Expires = DateTime.Now.AddMinutes(20) };
				Response.Cookies.Add(cookie);
			}
		}
	}
}
