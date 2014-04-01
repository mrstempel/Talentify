using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models
{
	public class DBMigrationHistory : BaseEntity
	{
		public DateTime CreatedOn { get; set; }
		public string Name { get; set; }
		public bool IsDone { get; set; }
	}

	public class DBMigrationHistoryMap : EntityTypeConfiguration<DBMigrationHistory>
	{
		public DBMigrationHistoryMap()
		{
			// Table Name
			this.ToTable("DBMigrationHistory");
			// Primary Key
			this.HasKey(t => t.Id);
			//
			this.Property(f => f.Name).HasMaxLength(250).IsRequired();
		}
	}
}
