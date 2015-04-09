using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.Notification;
using Talentify.ORM.DAL.Models.Talentecheck;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public enum TalentecheckBonusAction
	{
		Register,
		ShareFb,
		ShareTw,
		ShareGoogle,
		Invite
	}

	public static class TalentecheckBonusPointsFor
	{
		public const int Register = 100;
		public const int Share = 50;
		public const int Invite = 100;
	}

	public class TalentecheckBonusRepository : TalentifyRepository<TalentecheckBonus>
	{
		public TalentecheckBonusRepository(TalentifyContext context)
            : base(context)
        {
        }

		public override void Insert(TalentecheckBonus entity)
		{
			base.Insert(entity);

			// update highscores
			var highscores =
				UnitOfWork.TalentecheckHighscoreRepository.AsQueryable()
					.FirstOrDefault(h => h.TalentecheckSessionId == entity.TalentecheckSessionId);
			if (highscores == null)
			{
				highscores = new TalentecheckHighscore() {TalentecheckSessionId = entity.TalentecheckSessionId};
				highscores.Points = entity.Points;
				UnitOfWork.TalentecheckHighscoreRepository.Insert(highscores);
			}
			else
			{
				highscores.Points = highscores.Points + entity.Points;
				UnitOfWork.TalentecheckHighscoreRepository.Update(highscores);
			}
		}
	}
}
