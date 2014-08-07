using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Notification
{
	public enum NotificationSenderType
	{
		CoachingRequest,
		User,
		Event,
		System
	}

	public enum NotificationIconType
	{
		None,
		Confirmed,
		Cancelled,
		Bonus,
		Badge
	}

	public class Notification : BaseEntity
	{
		public int ToUserId { get; set; }
		private BaseUser _toUser;
		public virtual BaseUser ToUser
		{
			get { return _toUser; }
			set
			{
				_toUser = value;
				if (value != null)
					ToUserId = value.Id;
			}
		}

		public int SenderId { get; set; }
		public int TargetId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? ReadDate { get; set; }
		public int Bonus { get; set; }
		public NotificationSenderType SenderType { get; set; }
		public NotificationIconType IconType { get; set; }
		public string Text { get; set; }
		public string AdditionalInfo { get; set; }
	}

	public class NotificationMap : EntityTypeConfiguration<Notification>
	{
		public NotificationMap()
		{
			// Table Name
			this.ToTable("Notification");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime2");
			this.Property(t => t.ReadDate).HasColumnName("ReadDate").HasColumnType("datetime2");
		}
	}
}
