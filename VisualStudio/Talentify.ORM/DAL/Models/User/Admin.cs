using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talentify.ORM.DAL.Models.User
{
	public class Admin : BaseUser
	{

	}

	public class AdminMap : EntityTypeConfiguration<Admin>
	{
		public AdminMap()
		{
			// Table Name
			this.ToTable("Admin");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
