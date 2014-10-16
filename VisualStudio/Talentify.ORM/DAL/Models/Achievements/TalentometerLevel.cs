using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.Achievements
{
	public class TalentometerLevel : BaseEntity
	{
		public int Level { get; set; }
		public string Name { get; set; }
		public int MinPoints { get; set; }
		public int ConfirmedEvents { get; set; }
	}

	public class TalentometerLevelMap : EntityTypeConfiguration<TalentometerLevel>
	{
		public TalentometerLevelMap()
		{
			// Table Name
			this.ToTable("TalentometerLevel");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
