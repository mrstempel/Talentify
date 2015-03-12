using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Microsoft.SqlServer.Server;
using Talentify.ORM.DAL.Models.Content;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Feedback
{
	public class EventFeedback : BaseEntity
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

		public int Liked { get; set; }
		public int Helpful { get; set; }
		public bool RecommendWorthy { get; set; }
		public string Comments { get; set; }
		public DateTime CreatedDate { get; set; }
	}

	public class EventFeedbackMap : EntityTypeConfiguration<EventFeedback>
	{
		public EventFeedbackMap()
		{
			// Table Name
			this.ToTable("EventFeedback");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
