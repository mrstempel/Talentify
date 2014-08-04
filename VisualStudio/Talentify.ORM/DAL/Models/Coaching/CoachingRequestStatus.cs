using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Library;

namespace Talentify.ORM.DAL.Models.Coaching
{
	public enum StatusType
	{
		Request,
		Appointment,
		Completed,
		Canceled,
		Conflicted,
		Rejected 
	}

	public class CoachingRequestStatus : BaseEntity, ICoachingRequestTimelineItem
	{
		public int CoachingRequestId { get; set; }
		private CoachingRequest _coachingRequest;
		public virtual CoachingRequest CoachingRequest
		{
			get { return _coachingRequest; }
			set
			{
				_coachingRequest = value;
				if (value != null)
					CoachingRequestId = value.Id;
			}
		}

		public int CreatedById { get; set; }
		private BaseUser _createdBy;
		public virtual BaseUser CreatedBy
		{
			get { return _createdBy; }
			set
			{
				_createdBy = value;
				if (value != null)
					CreatedById = value.Id;
			}
		}

		public StatusType StatusType { get; set; }
		public string Text { get; set; }
		public DateTime CreatedDate { get; set; }

		#region ICoachingRequestTimelineItem Implementation

		public int UserId
		{
			get { return CreatedById; }
		}

		public string UserImage
		{
			get { return CreatedBy != null ? CreatedBy.ProfileImageSmall : null; }
		}

		public string Username
		{
			get { return CreatedBy != null ? CreatedBy.Firstname + " " + CreatedBy.Surname : null; }
		}

		public TimelineItemType ItemType
		{
			get
			{
				//if (StatusType == StatusType.Completed)
				//	return TimelineItemType.Bonus;
				if (StatusType == StatusType.Canceled || StatusType == StatusType.Rejected)
					return TimelineItemType.Canceled;

				return TimelineItemType.Confirmation;
			}
		}

		#endregion
	}

	public class CoachingRequestStatusMap : EntityTypeConfiguration<CoachingRequestStatus>
	{
		public CoachingRequestStatusMap()
		{
			// Table Name
			this.ToTable("CoachingRequestStatus");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime2");
		}
	}
}
