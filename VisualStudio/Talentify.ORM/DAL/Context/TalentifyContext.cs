using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Context;
using Talentify.ORM.DAL.Models;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Content;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.Messaging;
using Talentify.ORM.DAL.Models.Notification;
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
		public DbSet<CoachingOffer> CoachingOffers { get; set; }
		public DbSet<CoachingRequest> CoachingRequests { get; set; }

		// users
		public DbSet<BaseUser> BasUsers { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<Admin> Admins { get; set; }
		public DbSet<ActionToken> ActionToken { get; set; }
		public DbSet<UserSettings> UserSettings { get; set; }
		public DbSet<RegisterCode> RegisterCodes { get; set; }

		// memberships
		public DbSet<Talentify.ORM.DAL.Models.Membership.Membership> Memberships { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }

		// content
		public DbSet<BasePage> BasePages { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<EventRegistration> EventRegistrations { get; set; }

		// messaging
		public DbSet<Conversation> Conversations { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<MessageRecipient> MessageRecipients { get; set; }

		// notification
		public DbSet<Notification> Notifications { get; set; }

		// achievements
		public DbSet<BonusPoint> BonusPoints { get; set; }
		public DbSet<Badge> Badges { get; set; }
		public DbSet<TalentometerLevel> TalentometerLevel { get; set; }
		
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
			modelBuilder.Configurations.Add(new CoachingOfferMap());
			modelBuilder.Configurations.Add(new CoachingRequestMap());
			modelBuilder.Configurations.Add(new CoachingRequestStatusMap());

			// users
			modelBuilder.Configurations.Add(new BaseUserMap());
			modelBuilder.Configurations.Add(new StudentMap());
			modelBuilder.Configurations.Add(new TeacherMap());
			modelBuilder.Configurations.Add(new AdminMap());
			modelBuilder.Configurations.Add(new ActionTokenMap());
			modelBuilder.Configurations.Add(new UserSettingsMap());
			modelBuilder.Configurations.Add(new RegisterCodeMap());

			// memberships
			modelBuilder.Configurations.Add(new MembershipMap());
			modelBuilder.Configurations.Add(new SubscriptionMap());

			// content
			modelBuilder.Configurations.Add(new BasePageMap());
			modelBuilder.Configurations.Add(new EventMap());
			modelBuilder.Configurations.Add(new EventRegistrationMap());

			// messaging
			modelBuilder.Configurations.Add(new ConversationMap());
			modelBuilder.Configurations.Add(new MessageMap());
			modelBuilder.Configurations.Add(new MessageRecipientMap());

			// notifications
			modelBuilder.Configurations.Add(new NotificationMap());

			// achievements
			modelBuilder.Configurations.Add(new BonusPointMap());
			modelBuilder.Configurations.Add(new BadgeMap());
			modelBuilder.Configurations.Add(new TalentometerLevelMap());
			
			base.OnModelCreating(modelBuilder);
		}
	}
}
