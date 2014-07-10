using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.Notification;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class NotificationRepository : TalentifyRepository<Notification>
	{
		public NotificationRepository(TalentifyContext context)
            : base(context)
        {
        }

		public int Count(int userId)
		{
			return Convert.ToInt32(UnitOfWork.NotificationRepository.AsQueryable().Count(n => n.ToUserId == userId && n.ReadDate == null));
		}

		private IEnumerable<NotificationListItem> ConvertToListItems(IEnumerable<Notification> notifictions)
		{
			return null;
		}

		public IEnumerable<NotificationListItem> GetPopupList(int userId)
		{
			var notifications = UnitOfWork.NotificationRepository.Get().OrderByDescending(n => n.CreatedDate).Take(5);
			return ConvertToListItems(notifications);
		}
	}
}
