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
	public class Badge : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Icon { get; set; }

		public ICollection<BaseUser> Users { get; set; } 
	}

	public class BadgeMap : EntityTypeConfiguration<Badge>
	{
		public BadgeMap()
		{
			// Table Name
			this.ToTable("Badge");
			// Primary Key
			this.HasKey(t => t.Id);

			// user-groups relation
			this.HasMany(t => t.Users)
				.WithMany(t => t.Badges)
				.Map(m => { m.ToTable("BadgesToBaseUser"); });
		}
	}
}
