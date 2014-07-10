using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using Talentify.ORM.DAL.Context;
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

		public IEnumerable<Event> GetOnlineEvents()
		{
			return UnitOfWork.EventRepository.AsQueryable().Where(e => e.IsOnline && e.BeginDate >= DateTime.Now);
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

		public int GetOpenSeats(int eventId)
		{
			var talentiyEvent = UnitOfWork.EventRepository.GetById(eventId);
			var registrations = UnitOfWork.EventRegistrationRepository.AsQueryable().Where(reg => reg.EventId == eventId);
			return talentiyEvent.MaxParticipant - registrations.Count();
		}

		public bool AddRegistration(int eventId, int userId)
		{
			if (GetOpenSeats(eventId) > 0)
			{
				var registration = new EventRegistration() {UserId = userId, EventId = eventId, CreatedDate = DateTime.Now};
				UnitOfWork.EventRegistrationRepository.Insert(registration);
				UnitOfWork.Save();
				return true;
			}

			return false;
		}

		public bool IsUserRegistered(int eventId, int userId)
		{
			var registration =
				UnitOfWork.EventRegistrationRepository.AsQueryable()
					.FirstOrDefault(reg => reg.EventId == eventId && reg.UserId == userId);
			return (registration != null);
		}
	}
}
