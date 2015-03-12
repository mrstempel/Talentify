using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Achievements
{
	public static class BonusPointsFor
	{
		public const int Register = 10;
		public const int ProfileFull = 10;
		public const int InitialCoachingOffer = 20;
		public const int CoachingOffer = 10;
		public const int CoachingOfferCompleted = 5;
		public const int LowPrice = 5;
		public const int Invite = 10;
		public const int Survey = 10;
		public const int EventConfirm = 10;
		public const int EventFeedback = 10;
	}

	public class BonusPoint : BaseEntity
	{
		public int UserId { get; set; }
		private BaseUser _user;
		public BaseUser User
		{
			get { return _user; }
			set
			{
				_user = value;
				if (value != null)
					UserId = value.Id;
			}
		}

		public int Points { get; set; }
		public string Message { get; set; }
		public DateTime CreatedDate { get; set; }
	}

	public class BonusPointMap : EntityTypeConfiguration<BonusPoint>
	{
		public BonusPointMap()
		{
			// Table Name
			this.ToTable("BonusPoint");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime2");
		}
	}
}
