using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.User
{
	public class BaseUser : BaseEntity
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Firstname { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
		public string Address { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public DateTime JoinedDate { get; set; }
		public DateTime UpdateDate { get; set; }
	}

	public class BaseUserMap : EntityTypeConfiguration<BaseUser>
	{
		public BaseUserMap()
		{
			// Table Name
			this.ToTable("BaseUser");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
