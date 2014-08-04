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
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class BonusPointRepository : TalentifyRepository<BonusPoint>
	{
		public BonusPointRepository(TalentifyContext context)
            : base(context)
        {
        }

		public void Insert(int userId, int points, string message)
		{
			// add bonus points
			var bonusPoints = new BonusPoint()
			{
				UserId = userId,
				Points = points,
				Message = message,
				CreatedDate = DateTime.Now
			};
			Insert(bonusPoints);
			// set notification
			var notifiction = new Notification()
			{
				ToUserId = userId,
				SenderId = 0,
				TargetId = 0,
				Text = message,
				CreatedDate = DateTime.Now,
				SenderType = NotificationSenderType.System,
				IconType = NotificationIconType.Bonus,
				Bonus = points
			};
			UnitOfWork.NotificationRepository.Insert(notifiction);
			// save
			UnitOfWork.Save();
		}
	}
}
