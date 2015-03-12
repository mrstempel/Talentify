using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Timers;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Notification;
using Talentify.ORM.DAL.UnitOfWork;
using Talentify.ORM.Utils;

namespace Talentify.ORM.Timers
{
	public class EventEmailTimer
	{
		static Timer _timer;
		static string _machineName;

		public static void Start(string machineName)
		{
			_machineName = machineName;
			_timer = new Timer(TalentifySettings.Default.EventNotificationTimerRepeat);
			_timer.Elapsed += TimerOnElapsed;
			_timer.Enabled = true;
		}

		private static void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			_timer.Enabled = false;

			if (TalentifySettings.Default.EventNotificationTimerRepeatActive)
			{
				var unitOfWork = new TalentifyUnitOfWork<TalentifyContext>();
				HandleNotificationEmails(unitOfWork);
				unitOfWork.Dispose();
			}

			_timer.Enabled = true;
		}

		private static void HandleNotificationEmails(TalentifyUnitOfWork<TalentifyContext> unitOfWork)
		{
			var events = unitOfWork.EventRepository.GetNotificationEvents();
			foreach (var e in events)
			{
				try
				{
					var notifiyUsers = unitOfWork.EventRepository.GetNotifyUsers(e.Id).ToList();

					foreach (var user in notifiyUsers)
					{
						// set plattform notification
						var notification = new Notification()
						{
							CreatedDate = DateTime.Now,
							IconType = NotificationIconType.None,
							TargetId = e.Id,
							ToUserId = user.Id,
							SenderType = NotificationSenderType.Event,
							Text = string.Format("Reminder Workshop am {0}", e.BeginDate.ToString("d.M.yyyy", new CultureInfo("de")))
						};
						unitOfWork.NotificationRepository.InsertNoEmail(notification);
						// send email
						var tag = e.BeginDate.ToString("dddd", new CultureInfo("de"));
						var datum = e.BeginDate.ToString("d.M.yyyy", new CultureInfo("de"));
						var address = string.Format("{0}<br/>{1}<br/>{2} {3}<br/>{4}", e.LocationName, e.Address, e.ZipCode, e.City,
							e.Country);
						Email.SenEventNotification(user.Email, ConfigurationManager.AppSettings["Email.Notifiction.Subject"], e.Title, tag, datum, e.BeginTime, e.EndTime, address, e.Link);
						// update notification flag in registration
						unitOfWork.EventRegistrationRepository.MarkAsNotified(e.Id, user.Id);
						unitOfWork.Save();
					}
				}
				catch (Exception ex)
				{
				}
			}
		}
	}
}
