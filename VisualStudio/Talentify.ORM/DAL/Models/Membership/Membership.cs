using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.Membership
{
	public enum MembershipType
	{
		Free	
	}

	public class Membership : BaseEntity
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public int ValidDays { get; set; }
		public MembershipType Type { get; set; }
	}

	public class MembershipMap : EntityTypeConfiguration<Membership>
	{
		public MembershipMap()
		{
			// Table Name
			this.ToTable("BaseMembership");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
