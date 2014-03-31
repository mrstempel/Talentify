using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.School
{
	public class SchoolType : BaseEntity
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public int StartClass { get; set; }
		public int EndClass { get; set; }
	}

	public class SchoolTypeMap : EntityTypeConfiguration<SchoolType>
	{
		public SchoolTypeMap()
		{
			// Table Name
			this.ToTable("SchoolType");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
