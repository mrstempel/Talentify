using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talentify.ORM.DAL.Models.Content
{
	public class Event : BasePage
	{
		public DateTime BeginDate { get; set; }
		public string BeginTime { get; set; }
		public string EndTime { get; set; }
		public string LocationName { get; set; }
		public string Address { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public int MaxParticipant { get; set; }
	}

	public class EventMap : EntityTypeConfiguration<Event>
	{
		public EventMap()
		{
			// Table Name
			this.ToTable("Event");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.BeginDate).HasColumnName("JoinedDate").HasColumnType("datetime2");
		}
	}
}
