using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Tracking
{
	public class TrackingClick : BaseEntity
	{
		public int UserId { get; set; }
		public string Link { get; set; }
		public DateTime InsertDate { get; set; }
	}

	public class TrackingClickMap : EntityTypeConfiguration<TrackingClick>
	{
		public TrackingClickMap()
		{
			// Table Name
			this.ToTable("TrackingClick");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.InsertDate).HasColumnName("InsertDate").HasColumnType("datetime2");
		}
	}
}
