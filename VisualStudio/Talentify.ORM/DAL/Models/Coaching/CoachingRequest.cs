using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.Messaging;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Coaching
{
	public class CoachingRequest : BaseEntity
	{
		public int FromUserId { get; set; }
		private Student _fromUser;
		public virtual Student FromUser
		{
			get { return _fromUser; }
			set
			{
				_fromUser = value;
				if (value != null)
					FromUserId = value.Id;
			}
		}

		public int ToUserId { get; set; }
		private Student _toUser;
		public virtual Student ToUser
		{
			get { return _toUser; }
			set
			{
				_toUser = value;
				if (value != null)
					ToUserId = value.Id;
			}
		}

		public int SubjectCategoryId { get; set; }
		private SubjectCategory _subjectCategory;
		public virtual SubjectCategory SubjectCategory
		{
			get { return _subjectCategory; }
			set
			{
				_subjectCategory = value;
				if (value != null)
					SubjectCategoryId = value.Id;
			}
		}

		public DateTime CreatedDate { get; set; }
		public int Class { get; set; }
		public decimal Price { get; set; }
		public virtual ICollection<CoachingRequestStatus> StatusHistory { get; set; }
		public virtual ICollection<CoachingRating> Ratings { get; set; }
		public virtual Conversation Conversation { get; set; }

		public DateTime? Date { get; set; }
		public decimal Duration { get; set; }
		public decimal PayedPrice { get; set; }
	}

	public class CoachingRequestMap : EntityTypeConfiguration<CoachingRequest>
	{
		public CoachingRequestMap()
		{
			// Table Name
			this.ToTable("CoachingRequest");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime2");
			this.Property(t => t.Date).HasColumnName("Date").HasColumnType("datetime2");
		}
	}
}
