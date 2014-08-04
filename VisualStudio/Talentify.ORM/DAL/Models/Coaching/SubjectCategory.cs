using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Library;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Coaching
{
	public class SubjectCategory : BaseEntity, IFormCheckable
	{
		public string Name { get; set; }
		public ICollection<Teacher> Teachers { get; set; } 

		#region Implement IFormCheckable

		public string FormLabel
		{
			get { return Name; }
		}

		#endregion
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
