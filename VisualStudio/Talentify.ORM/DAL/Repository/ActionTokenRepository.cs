using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class ActionTokenRepository : TalentifyRepository<ActionToken>
	{
		public ActionTokenRepository(TalentifyContext context)
            : base(context)
        {
        }

		public ActionToken Get(Guid guid, ActionTokenType type)
		{
			return AsQueryable().FirstOrDefault(t => t.Token == guid && t.Type == type && t.ValidUntil >= DateTime.Now);
		}

		public ActionToken GetInviteToken(int userId)
		{
			var token = AsQueryable().FirstOrDefault(t => t.UserId == userId && t.Type == ActionTokenType.Invite);

			if (token == null)
			{
				token = new ActionToken()
				{
					UserId = userId,
					Type = ActionTokenType.Invite,
					CreatedDate = DateTime.Now,
					ValidUntil = DateTime.MaxValue,
					Token = Guid.NewGuid()
				};
				Insert(token);
				UnitOfWork.Save();
			}

			return token;
		}

		public ActionToken GetSurveyToken(int userId)
		{
			var token = AsQueryable().FirstOrDefault(t => t.UserId == userId && t.Type == ActionTokenType.Survey);

			if (token == null)
			{
				token = new ActionToken()
				{
					UserId = userId,
					Type = ActionTokenType.Survey,
					CreatedDate = DateTime.Now,
					ValidUntil = DateTime.MaxValue,
					Token = Guid.NewGuid()
				};
				Insert(token);
				UnitOfWork.Save();
			}

			return token;
		}
	}
}
