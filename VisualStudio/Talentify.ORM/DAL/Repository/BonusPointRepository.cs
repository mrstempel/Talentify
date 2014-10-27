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
			Insert(userId, points, message, 0);
		}

		public void Insert(int userId, int points, string message, int targetId)
		{
			Insert(userId, points, message, targetId, true);
		}

		public void Insert(int userId, int points, string message, int targetId, bool saveChanges)
		{
			Insert(userId, points, message, targetId, saveChanges, null);
		}

		public void Insert(int userId, int points, string message, int targetId, bool saveChanges, string additionalInfo)
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
				TargetId = targetId,
				Text = message,
				CreatedDate = DateTime.Now,
				SenderType = NotificationSenderType.System,
				IconType = NotificationIconType.Bonus,
				Bonus = points,
				AdditionalInfo = additionalInfo
			};
			UnitOfWork.NotificationRepository.Insert(notifiction);
			// save
			if (saveChanges)
			{
				UnitOfWork.Save();
			}
		}

		public long GetUserBonus(int userId)
		{
			try
			{
				return (from b in UnitOfWork.BonuspointRepository.AsQueryable()
						   where
							   b.UserId == userId
						   select (long)b.Points).Sum();
			}
			catch (Exception)
			{
				return 0;
			}
		}
	}
}
