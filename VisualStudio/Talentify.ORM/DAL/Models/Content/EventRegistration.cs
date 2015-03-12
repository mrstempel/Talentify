using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Content
{
	public class EventRegistration : BaseEntity
	{
		public int UserId { get; set; }
		private BaseUser _user;
		public virtual BaseUser User
		{
			get { return _user; }
			set
			{
				_user = value;
				if (value != null)
					UserId = value.Id;
			}
		}

		public int EventId { get; set; }
		private Event _event;
		public virtual Event Event
		{
			get { return _event; }
			set
			{
				_event = value;
				if (value != null)
					EventId = value.Id;
			}
		}

		public DateTime CreatedDate { get; set; }
		public bool Confirmed { get; set; }
		public bool IsSignedOff { get; set; }
		public int Price { get; set; }
		public int Bonuspoints { get; set; }
		public bool WasNotified { get; set; }
		public bool HasFollowUpEmail { get; set; }
		public string Comments { get; set; }
	}

	public class EventRegistrationMap : EntityTypeConfiguration<EventRegistration>
	{
		public EventRegistrationMap()
		{
			// Table Name
			this.ToTable("EventRegistration");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
