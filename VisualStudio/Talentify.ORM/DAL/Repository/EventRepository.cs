using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Content;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class EventRepository : BasePageRepository<Event> 
	{
		public EventRepository(TalentifyContext context)
            : base(context)
        {
        }

		public void Insert(Event entity, HttpPostedFileBase uploadPreview, HttpPostedFileBase uploadImage, HttpPostedFileBase homeImage, string uploadPath)
		{
			var hours = Convert.ToInt16(entity.BeginTime.Substring(0, 2));
			var minutes = Convert.ToInt16(entity.BeginTime.Substring(3, 2));
			entity.BeginDate = entity.BeginDate.AddHours(hours).AddMinutes(minutes);

			// save home image
			if (homeImage.ContentLength > 0)
			{
				entity.HomeImage = Guid.NewGuid() + Path.GetExtension(homeImage.FileName);
				homeImage.SaveAs(Path.Combine(uploadPath, entity.HomeImage));
			}

			base.Insert(entity, uploadPreview, uploadImage, uploadPath);
		}

		public void Update(Event entity, HttpPostedFileBase uploadPreview, HttpPostedFileBase uploadImage, HttpPostedFileBase homeImage, string uploadPath)
		{
			var hours = Convert.ToInt16(entity.BeginTime.Substring(0, 2));
			var minutes = Convert.ToInt16(entity.BeginTime.Substring(3, 2));
			entity.BeginDate = new DateTime(entity.BeginDate.Year, entity.BeginDate.Month, entity.BeginDate.Day, hours, minutes, 0);
			// save home image
			if (homeImage != null && homeImage.ContentLength > 0)
			{
				entity.HomeImage = Guid.NewGuid() + Path.GetExtension(homeImage.FileName);
				homeImage.SaveAs(Path.Combine(uploadPath, entity.HomeImage));
			}

			base.Update(entity, uploadPreview, uploadImage, uploadPath);
		}

		public IEnumerable<Event> GetOnlineEvents()
		{
			return UnitOfWork.EventRepository.AsQueryable().Where(e => e.IsOnline && e.BeginDate >= DateTime.Now).OrderBy(e => e.BeginDate);
		}

		public EventOverview GetEventOverview(int userId)
		{
			var onlineEvents = GetOnlineEvents();
			var myRegistrations = UnitOfWork.EventRegistrationRepository.AsQueryable().Where(r => r.UserId == userId).ToList();
			var eventOverView = new EventOverview();
			foreach (var e in onlineEvents)
			{
				if (myRegistrations.Any(r => r.EventId == e.Id))
				{
					eventOverView.MyEvents.Add(e);
				}
				else
				{
					eventOverView.Events.Add(e);
				}
			}

			return eventOverView;
		}

		public IEnumerable<Event> Filter(string filter, int userId)
		{
			var events = UnitOfWork.EventRepository.Get(e => e.IsOnline);

			if (filter == "future")
			{
				return events.Where(e => e.BeginDate >= DateTime.Now).OrderBy( e => e.BeginDate);
			}
			
			// my events
			if (filter == "my")
			{
				return from e in events join
						myRegistrations in UnitOfWork.EventRegistrationRepository.AsQueryable().Where(r => r.UserId == userId) on e.Id equals myRegistrations.EventId
						orderby e.BeginDate
					select e;
			}
			
			// specific event types
			if (filter.StartsWith("type"))
			{
				int typeInt = Convert.ToInt32(filter.Substring(filter.Length - 1));
				var type = (EventType) typeInt;
				return events.Where(e => e.Type == type);
			}

			if (filter == "past")
			{
				return events.Where(e => e.BeginDate < DateTime.Now).OrderBy(e => e.BeginDate);
			}

			return events;
		}

		public IEnumerable<Event> GetNextEvents(int currentId)
		{
			var nextEvents =
				AsQueryable()
					.Where(e => e.BeginDate > DateTime.Now && e.Id != currentId)
					.OrderByDescending(e => e.BeginDate)
					.Take(2);
			return nextEvents;
		}

		public int GetOpenSeats(int eventId)
		{
			var talentiyEvent = UnitOfWork.EventRepository.GetById(eventId);
			var registrations = UnitOfWork.EventRegistrationRepository.AsQueryable().Where(reg => reg.EventId == eventId);
			return talentiyEvent.MaxParticipant - registrations.Count();
		}

		public bool CancelRegistration(int eventId, int userId)
		{
			var registration =
				UnitOfWork.EventRegistrationRepository.AsQueryable().FirstOrDefault(r => r.EventId == eventId && r.UserId == userId);
			
			if (registration != null)
			{
				UnitOfWork.EventRegistrationRepository.SignOff(registration);
				UnitOfWork.Save();
			}

			return true;
		}

		public bool AddRegistration(int eventId, int userId, int price, int bonuspoints)
		{
			if (GetOpenSeats(eventId) > 0)
			{
				var registration =
					UnitOfWork.EventRegistrationRepository.AsQueryable()
						.FirstOrDefault(r => r.EventId == eventId && r.UserId == userId);

				if (registration == null)
				{
					registration = new EventRegistration() {UserId = userId, EventId = eventId, CreatedDate = DateTime.Now, Price = price, Bonuspoints = bonuspoints};
					UnitOfWork.EventRegistrationRepository.Insert(registration);
					UnitOfWork.Save();

					if (bonuspoints != 0)
					{
						// subtract bonus
						UnitOfWork.BonuspointRepository.Insert(userId, bonuspoints * -1, "Bonuspunkte für Event gesetzt", 0, true, null);
						UnitOfWork.Save();
					}
				}
				else if (registration.IsSignedOff)
				{
					registration.IsSignedOff = false;
					registration.CreatedDate = DateTime.Now;
					UnitOfWork.EventRegistrationRepository.Update(registration);
					UnitOfWork.Save();

					if (bonuspoints != 0)
					{
						// subtract bonus
						UnitOfWork.BonuspointRepository.Insert(userId, bonuspoints * -1, "Bonuspunkte für Event gesetzt", 0, true, null);
						UnitOfWork.Save();
					}
				}

				return true;
			}

			return false;
		}

		public bool IsUserRegistered(int eventId, int userId)
		{
			var registration =
				UnitOfWork.EventRegistrationRepository.AsQueryable()
					.FirstOrDefault(reg => reg.EventId == eventId && reg.UserId == userId && !reg.IsSignedOff);
			return (registration != null);
		}

		public List<int> GetUserRegisteredEventIds(int userId)
		{
			return
				(from regs in UnitOfWork.EventRegistrationRepository.AsQueryable().Where(reg => reg.UserId == userId && !reg.IsSignedOff)
					select regs.EventId).ToList();
		}

		public IEnumerable<EventRegistration> GetRegistrations(int eventId)
		{
			return UnitOfWork.EventRegistrationRepository.AsQueryable().Where(r => r.EventId == eventId && !r.IsSignedOff);
		}

		public IEnumerable<EventRegistration> GetRegistrationsSignedOff(int eventId)
		{
			return UnitOfWork.EventRegistrationRepository.AsQueryable().Where(r => r.EventId == eventId && r.IsSignedOff);
		}

		public IEnumerable<Event> GetNotificationEvents()
		{
			var checkDate = DateTime.Now.AddDays(1);
			var events = AsQueryable().Where(e => e.BeginDate < checkDate && e.BeginDate > DateTime.Now && e.IsOnline).ToList();
			return events;
		}

		public IEnumerable<BaseUser> GetNotifyUsers(int eventId)
		{
			var users = from u in UnitOfWork.BaseUserRepository.AsQueryable()
						join
							r in UnitOfWork.EventRegistrationRepository.AsQueryable().Where(r => r.EventId == eventId && !r.IsSignedOff && !r.WasNotified) on
							u.Id equals r.UserId
						select u;
			
			return users;
		}

		public IEnumerable<BaseUser> GetFollowUpUsers(int eventId)
		{
			var users = from u in UnitOfWork.BaseUserRepository.AsQueryable()
						join
							r in UnitOfWork.EventRegistrationRepository.AsQueryable().Where(r => r.EventId == eventId && !r.IsSignedOff && !r.HasFollowUpEmail) on
							u.Id equals r.UserId
						select u;
			return users;
		}

		public void SendFollowUpEmail(int eventId)
		{
			var e = GetById(eventId);
			var users = GetFollowUpUsers(eventId).ToList();
			var registrations = GetRegistrations(eventId).Where(r => !r.HasFollowUpEmail).ToList();

			foreach (var user in users)
			{
				var reg = registrations.FirstOrDefault(r => r.UserId == user.Id);
				if (reg != null)
				{
					
					if (reg.Confirmed)
					{
						// confirmed follow ups
						Email.SenEventConfirmedEmail(user.Email, ConfigurationManager.AppSettings["Email.Notifiction.Subject"], e.Title, e.Id);
					}
					else
					{
						// not confirmed follow ups
						// check how often the user has not attended
						var notAttendedCount = UnitOfWork.EventRegistrationRepository.GetNotAttendedCount(user.Id);
						if (notAttendedCount == 0)
						{
							var tag = e.BeginDate.ToString("dddd", new CultureInfo("de"));
							var datum = e.BeginDate.ToString("d.M.yyyy", new CultureInfo("de"));
							Email.SenEventNotConfirmed1Email(user.Email, ConfigurationManager.AppSettings["Email.Notifiction.Subject"],
								e.Title, tag, datum, e.BeginTime, e.EndTime, reg.Id);
						}
						else
						{
							var tag = e.BeginDate.ToString("dddd", new CultureInfo("de"));
							var datum = e.BeginDate.ToString("d.M.yyyy", new CultureInfo("de"));
							Email.SenEventNotConfirmed2Email(user.Email, ConfigurationManager.AppSettings["Email.Notifiction.Subject"],
								e.Title, tag, datum, e.BeginTime, e.EndTime);
							user.IsWorkshopBlocked = true;
							UnitOfWork.BaseUserRepository.Update(user);
						}
					}

					reg.HasFollowUpEmail = true;
					UnitOfWork.EventRegistrationRepository.Update(reg);
				}
			}

			UnitOfWork.Save();
		}
	}
}
