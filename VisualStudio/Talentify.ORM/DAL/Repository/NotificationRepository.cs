﻿using System;
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
							IsNew = notification.ReadDate == null
						};
						listItems.Add(item);
					}

					if (notification.IconType == NotificationIconType.Bonus)
					{
						var item = new NotificationListItem
						{
							Image = "/Images/sender-talentify.png",
							Link = "/Profile",
							Text = notification.Text,
							IconType = notification.IconType,
							IsNew = notification.ReadDate == null,
							Bonus = notification.Bonus
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
	}
}
