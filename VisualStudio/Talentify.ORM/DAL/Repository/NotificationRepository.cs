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

		public override void Insert(Notification entity)
		{
			base.Insert(entity);

			// check if email notifiction should be sent
			var user = UnitOfWork.BaseUserRepository.GetById(entity.ToUserId, "Settings");
			if (user.Settings != null && user.Settings.HasNotifications)
			{
				var icon = string.Empty;
				if (entity.IconType == NotificationIconType.Confirmed)
					icon = "icon-zusagen.png";
				if (entity.IconType == NotificationIconType.Cancelled)
					icon = "icon-absagen.png";
				//if (entity.IconType == NotificationIconType.Bonus)
				//	icon = "icon-bonus" + entity.Bonus + ".png";

				var link = entity.TargetId != 0 ? "/Profile/Index/" + entity.TargetId : "/Profile";
				var profileImage = "/Images/sender-talentify.png";
				if (entity.SenderType == NotificationSenderType.CoachingRequest)
				{
					var sender = UnitOfWork.BaseUserRepository.GetById(entity.SenderId);
					profileImage = sender.ProfileImageSmall;
					link = string.Format("{0}/{1}", ConfigurationManager.AppSettings["Notifications.SenderType." + (int) entity.SenderType + ".Url"], entity.TargetId);
				}

				var notifictionText = entity.Text;

				if (entity.Bonus != 0)
				{
					notifictionText += string.Format("<span style='float: right;background: #ef4d69;color: #fff;padding: 8px 4px;width: 35px;text-align: center;box-sizing: border-box;font-size: 15px;'>{0}{1}</span>", entity.Bonus > 0 ? "+" : string.Empty,
						entity.Bonus);
				}

				if ((entity.Text.StartsWith("Neue Nachricht von:") || entity.Text.StartsWith("Lernhilfeanfrage von:")) && !string.IsNullOrEmpty(entity.AdditionalInfo))
				{
					notifictionText += "<br/><br/>\"" + entity.AdditionalInfo + "\"";
				}

				Email.SendNotification(user.Email, ConfigurationManager.AppSettings["Email.Notifiction.Subject"], icon, link, profileImage, notifictionText);
			}
		}

		public void InsertNoEmail(Notification entity)
		{
			base.Insert(entity);
		}

		public int Count(int userId)
		{
			return Convert.ToInt32(UnitOfWork.NotificationRepository.AsQueryable().Count(n => n.ToUserId == userId && n.ReadDate == null));
		}

		private IEnumerable<NotificationListItem> ConvertToListItems(IEnumerable<Notification> notifictions)
		{
			if (notifictions.Any())
			{
				var listItems = new List<NotificationListItem>();
				var profileImages = new Dictionary<int, string>();
				foreach (var notification in notifictions)
				{
					if (notification.SenderType == NotificationSenderType.CoachingRequest)
					{
						if (!profileImages.ContainsKey(notification.SenderId))
						{
							var user = UnitOfWork.BaseUserRepository.GetById(notification.SenderId);
							profileImages.Add(notification.SenderId, user.ProfileImageSmall);
						}

						var item = new NotificationListItem
						{
							Image = profileImages[notification.SenderId],
							Link =
								string.Format("{0}/{1}",
									ConfigurationManager.AppSettings["Notifications.SenderType." + (int) notification.SenderType + ".Url"],
									notification.TargetId),
							Text = notification.Text,
							IconType = notification.IconType,
							IsNew = notification.ReadDate == null,
							CreatedDate = notification.CreatedDate
						};
						listItems.Add(item);
					}

					if (notification.IconType == NotificationIconType.Bonus)
					{
						var link = notification.TargetId != 0 ? "/Profile/Index/" + notification.TargetId : "/Profile";
						var item = new NotificationListItem
						{
							Image = "/Images/sender-talentify.png",
							Link = link,
							Text = notification.Text,
							IconType = notification.IconType,
							IsNew = notification.ReadDate == null,
							Bonus = notification.Bonus,
							CreatedDate = notification.CreatedDate
						};
						listItems.Add(item);
					}

					if (notification.IconType == NotificationIconType.Badge)
					{
						var item = new NotificationListItem
						{
							Image = "/Images/sender-talentify.png",
							Link = "/Profile",
							Text = notification.Text,
							IconType = notification.IconType,
							IsNew = notification.ReadDate == null,
							BadgeIcon = notification.AdditionalInfo,
							CreatedDate = notification.CreatedDate
						};
						listItems.Add(item);
					}

					if (notification.IconType == NotificationIconType.None && notification.SenderType == NotificationSenderType.Event)
					{
						var item = new NotificationListItem
						{
							Image = "/Images/sender-talentify.png",
							Link = "/Events/Detail/" + notification.TargetId,
							Text = notification.Text,
							IconType = notification.IconType,
							IsNew = notification.ReadDate == null,
							CreatedDate = notification.CreatedDate
						};
						listItems.Add(item);
					}

					notification.ReadDate = DateTime.Now;
					UnitOfWork.NotificationRepository.Update(notification);
				}

				UnitOfWork.Save();
				return listItems;
			}

			return null;
		}

		public IEnumerable<NotificationListItem> GetPopupList(int userId)
		{
			var notifications = UnitOfWork.NotificationRepository.Get(n => n.ToUserId == userId).OrderByDescending(n => n.CreatedDate).Take(5);
			return ConvertToListItems(notifications);
		}

		public IEnumerable<NotificationListItem> GetAll(int userId)
		{
			var notifications = UnitOfWork.NotificationRepository.Get(n => n.ToUserId == userId).OrderByDescending(n => n.CreatedDate);
			return ConvertToListItems(notifications);
		}
	}
}
