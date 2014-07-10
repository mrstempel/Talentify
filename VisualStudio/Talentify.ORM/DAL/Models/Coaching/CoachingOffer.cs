using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Coaching
{
	public class CoachingOffer : BaseEntity
	{
		public int UserId { get; set; }
		private Student _user;
		public Student User
		{
			get { return _user; }
			set
			{
				_user = value;
				if (value != null)
					UserId = value.Id;
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
		public DateTime UpdateDate { get; set; }
		public int FromClass { get; set; }
		public int ToClass { get; set; }
		public string Comments { get; set; }
	}

	public class CoachingOfferMap : EntityTypeConfiguration<CoachingOffer>
	{
		public CoachingOfferMap()
		{
			// Table Name
			this.ToTable("CoachingOffer");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime2");
			this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").HasColumnType("datetime2");
		}
	}
}
