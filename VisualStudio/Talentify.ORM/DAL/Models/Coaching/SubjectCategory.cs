using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.Coaching
{
	public class SubjectCategory : BaseEntity
	{
		public string Name { get; set; }
	}

	public class SubjectCategoryMap : EntityTypeConfiguration<SubjectCategory>
	{
		public SubjectCategoryMap()
		{
			// Table Name
			this.ToTable("SubjectCategory");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
