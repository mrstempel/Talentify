using System;
using System.Collections.Generic;
using System.IO;
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
	public class BadgeRepository : TalentifyRepository<Badge>
	{
		public BadgeRepository(TalentifyContext context)
            : base(context)
        {
        }

		public void AddBadgeToUser(BaseUser user, string badgeName)
		{
			if (user.Badges == null)
			{
				user.Badges = new List<Badge>();
			}

			var badge = AsQueryable().FirstOrDefault(b => b.Name == badgeName);
			if (badge != null)
			{
				user.Badges.Add(badge);
				UnitOfWork.BaseUserRepository.Update(user);

				// add badge notification
				var notifiction = new Notification()
				{
					ToUserId = user.Id,
					SenderId = 0,
					TargetId = 0,
					Text = string.Format("{0} Badge erhalten", badge.Name),
					CreatedDate = DateTime.Now,
					SenderType = NotificationSenderType.System,
					IconType = NotificationIconType.Badge,
					AdditionalInfo = badge.Icon
				};
				UnitOfWork.NotificationRepository.Insert(notifiction);
				UnitOfWork.Save();
			}
		}
	}
}
