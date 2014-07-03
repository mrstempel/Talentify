using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.User
{
	public class UserSettings : BaseEntity
	{
		public bool HasNotifications { get; set; }
		public bool HasNewsletter { get; set; }
	}

	public class UserSettingsMap : EntityTypeConfiguration<UserSettings>
	{
		public UserSettingsMap()
		{
			// Table Name
			this.ToTable("UserSettings");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
