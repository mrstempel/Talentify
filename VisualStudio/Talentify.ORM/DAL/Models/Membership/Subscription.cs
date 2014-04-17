using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Membership
{
	public class Subscription : BaseEntity
	{
		public int UserId { get; set; }
		private BaseUser _user;
		public BaseUser User
		{
			get { return _user; }
			set
			{
				_user = value;
				if (value != null)
					UserId = value.Id;
			}
		}

		public Membership Membership { get; set; }
		public DateTime PurchaseDate { get; set; }
	}

	public class SubscriptionMap : EntityTypeConfiguration<Subscription>
	{
		public SubscriptionMap()
		{
			// Table Name
			this.ToTable("Subscription");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
