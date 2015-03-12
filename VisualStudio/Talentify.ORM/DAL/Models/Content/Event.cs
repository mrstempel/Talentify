using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talentify.ORM.DAL.Models.Content
{
	public enum EventType
	{
		Academy,
		SocialSkillLab,
		OnTour,
		Tipp
	}

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
		public EventType Type { get; set; }
		public string HomeImage { get; set; }
		public string DetailUrl { get; set; }
		public int Bonuspoints { get; set; }
		public int Price { get; set; }
		public bool HasFollowUpCompleted { get; set; }

		#region Frontend Properties

		public string Link
		{
			get
			{
				return (!string.IsNullOrEmpty(DetailUrl)) ? string.Format("/Event/{0}", DetailUrl) : string.Format("/Events/Detail/{0}", Id);
			}
		}

		#endregion
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
