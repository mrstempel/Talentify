using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Context;
using Talentify.ORM.DAL.Models;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;
using Membership = System.Web.Security.Membership;

namespace Talentify.ORM.DAL.Context
{
	public class TalentifyContext : BaseContext
	{
		// migration
		public DbSet<DBMigrationHistory> DBMigrationHistory { get; set; }

		// schools
		public DbSet<SchoolType> SchoolTypes { get; set; }
		public DbSet<School> Schools { get; set; }

		// coaching
		public DbSet<SubjectCategory> SubjectCategories { get; set; }

		// users
		public DbSet<BaseUser> BasUsers { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<ActionToken> ActionToken { get; set; }

		// memberships
		public DbSet<Talentify.ORM.DAL.Models.Membership.Membership> Memberships { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }

		public TalentifyContext() : base("EFConnectionString")
		{
			this.Configuration.LazyLoadingEnabled = true;
			this.Configuration.ProxyCreationEnabled = true;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

			// migration
			modelBuilder.Configurations.Add(new DBMigrationHistoryMap());

			// schools
			modelBuilder.Configurations.Add(new SchoolTypeMap());
			modelBuilder.Configurations.Add(new SchoolMap());

			// coaching
			modelBuilder.Configurations.Add(new SubjectCategoryMap());

			// users
			modelBuilder.Configurations.Add(new BaseUserMap());
			modelBuilder.Configurations.Add(new StudentMap());
			modelBuilder.Configurations.Add(new TeacherMap());
			modelBuilder.Configurations.Add(new ActionTokenMap());

			// memberships
			modelBuilder.Configurations.Add(new MembershipMap());
			modelBuilder.Configurations.Add(new SubscriptionMap());

			base.OnModelCreating(modelBuilder);
		}
	}
}
