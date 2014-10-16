using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using KwIt.Project.Pattern.DAL.Library;
using KwIt.Project.Pattern.DAL.UnitOfWork;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Content;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.Messaging;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.DAL.Repository;

namespace Talentify.ORM.DAL.UnitOfWork
{
	public class TalentifyUnitOfWork<TContext>: GenericUnitOfWork<TContext>
        where TContext : TalentifyContext, new()
	{
		private IRepository<SchoolType> _schoolTypeRepository;
		public IRepository<SchoolType> SchoolTypeRepository
		{
			get
			{
				if (_schoolTypeRepository == null)
					_schoolTypeRepository = new TalentifyRepository<SchoolType>(this.Context);

				return _schoolTypeRepository;
			}
		}

		private SchoolRepository _schoolRepository;
		public SchoolRepository SchoolRepository
		{
			get
			{
				if (_schoolRepository == null)
					_schoolRepository = new SchoolRepository(this.Context);

				return _schoolRepository;
			}
		}

		private IRepository<SubjectCategory> _subjectCategoryRepository;
		public IRepository<SubjectCategory> SubjectCategoryRepository
		{
			get
			{
				if (_subjectCategoryRepository == null)
					_subjectCategoryRepository = new TalentifyRepository<SubjectCategory>(this.Context);

				return _subjectCategoryRepository;
			}
		}

		private CoachingOfferRepository _coachingOfferRepository;
		public CoachingOfferRepository CoachingOfferRepository
		{
			get
			{
				if (_coachingOfferRepository == null)
					_coachingOfferRepository = new CoachingOfferRepository(this.Context);

				return _coachingOfferRepository;
			}
		}

		private CoachingRequestRepository _coachingRequestRepository;
		public CoachingRequestRepository CoachingRequestRepository
		{
			get
			{
				if (_coachingRequestRepository == null)
					_coachingRequestRepository = new CoachingRequestRepository(this.Context);

				return _coachingRequestRepository;
			}
		}

		private IRepository<CoachingRequestStatus> _coachingRequestStatusRepository;
		public IRepository<CoachingRequestStatus> CoachingRequestStatusRepository
		{
			get
			{
				if (_coachingRequestStatusRepository == null)
					_coachingRequestStatusRepository = new TalentifyRepository<CoachingRequestStatus>(this.Context);

				return _coachingRequestStatusRepository;
			}
		}

		private BaseUserRepository<BaseUser> _baseUserRepository;
		public BaseUserRepository<BaseUser> BaseUserRepository
		{
			get
			{
				if (_baseUserRepository == null)
					_baseUserRepository = new BaseUserRepository<BaseUser>(this.Context);

				return _baseUserRepository;
			}
		}

		private StudentRepository _studentRepository;
		public StudentRepository StudentRepository
		{
			get
			{
				if (_studentRepository == null)
					_studentRepository = new StudentRepository(this.Context);

				return _studentRepository;
			}
		}

		private TeacherRepository _teacherRepository;
		public TeacherRepository TeacherRepository
		{
			get
			{
				if (_teacherRepository == null)
					_teacherRepository = new TeacherRepository(this.Context);

				return _teacherRepository;
			}
		}

		private BaseUserRepository<Admin> _adminRepository;
		public BaseUserRepository<Admin> AdminRepository
		{
			get
			{
				if (_adminRepository == null)
					_adminRepository = new BaseUserRepository<Admin>(this.Context);

				return _adminRepository;
			}
		}

		private ActionTokenRepository _actionTokenRepository;
		public ActionTokenRepository ActionTokenRepository
		{
			get
			{
				if (_actionTokenRepository == null)
					_actionTokenRepository = new ActionTokenRepository(this.Context);

				return _actionTokenRepository;
			}
		}

		private IRepository<UserSettings> _userSettingsRepository;
		public IRepository<UserSettings> UserSettingsRepository
		{
			get
			{
				if (_userSettingsRepository == null)
					_userSettingsRepository = new TalentifyRepository<UserSettings>(this.Context);

				return _userSettingsRepository;
			}
		}

		private RegisterCodeRepository _registerCodeRepository;
		public RegisterCodeRepository RegisterCodeRepository
		{
			get
			{
				if (_registerCodeRepository == null)
					_registerCodeRepository = new RegisterCodeRepository(this.Context);

				return _registerCodeRepository;
			}
		}

		private IRepository<Talentify.ORM.DAL.Models.Membership.Membership> _membershipRepository;
		public IRepository<Talentify.ORM.DAL.Models.Membership.Membership> MembershipRepository
		{
			get
			{
				if (_membershipRepository == null)
					_membershipRepository = new TalentifyRepository<Talentify.ORM.DAL.Models.Membership.Membership>(this.Context);

				return _membershipRepository;
			}
		}

		private IRepository<Subscription> _subscriptionRepository;
		public IRepository<Subscription> SubscriptionRepository
		{
			get
			{
				if (_subscriptionRepository == null)
					_subscriptionRepository = new TalentifyRepository<Subscription>(this.Context);

				return _subscriptionRepository;
			}
		}

		private BasePageRepository<BasePage> _basePageRepository;
		public BasePageRepository<BasePage> BasePageRepository
		{
			get
			{
				if (_basePageRepository == null)
					_basePageRepository = new BasePageRepository<BasePage>(this.Context);

				return _basePageRepository;
			}
		}

		private EventRepository _eventRepository;
		public EventRepository EventRepository
		{
			get
			{
				if (_eventRepository == null)
					_eventRepository = new EventRepository(this.Context);

				return _eventRepository;
			}
		}

		private EventRegistrationRepository _eventRegistrationRepository;
		public EventRegistrationRepository EventRegistrationRepository
		{
			get
			{
				if (_eventRegistrationRepository == null)
					_eventRegistrationRepository = new EventRegistrationRepository(this.Context);

				return _eventRegistrationRepository;
			}
		}

		private ConversationRepository _conversationRepository;
		public ConversationRepository ConversationRepository
		{
			get
			{
				if (_conversationRepository == null)
					_conversationRepository = new ConversationRepository(this.Context);

				return _conversationRepository;
			}
		}

		private IRepository<Message> _messageRepository;
		public IRepository<Message> MessageRepository
		{
			get
			{
				if (_messageRepository == null)
					_messageRepository = new TalentifyRepository<Message>(this.Context);

				return _messageRepository;
			}
		}

		private IRepository<MessageRecipient> _messageRecipientRepository;
		public IRepository<MessageRecipient> MessageRecipientRepository
		{
			get
			{
				if (_messageRecipientRepository == null)
					_messageRecipientRepository = new TalentifyRepository<MessageRecipient>(this.Context);

				return _messageRecipientRepository;
			}
		}

		private NotificationRepository _notificationRepository;
		public NotificationRepository NotificationRepository
		{
			get
			{
				if (_notificationRepository == null)
					_notificationRepository = new NotificationRepository(this.Context);

				return _notificationRepository;
			}
		}

		private BonusPointRepository _bonuspointRepository;
		public BonusPointRepository BonuspointRepository
		{
			get
			{
				if (_bonuspointRepository == null)
					_bonuspointRepository = new BonusPointRepository(this.Context);

				return _bonuspointRepository;
			}
		}

		private BadgeRepository _badgeRepository;
		public BadgeRepository BadgeRepository
		{
			get
			{
				if (_badgeRepository == null)
					_badgeRepository = new BadgeRepository(this.Context);

				return _badgeRepository;
			}
		}

		private TalentometerLevelRepository _talentometerLevelRepository;
		public TalentometerLevelRepository TalentometerLevelRepository
		{
			get
			{
				if (_talentometerLevelRepository == null)
					_talentometerLevelRepository = new TalentometerLevelRepository(this.Context);

				return _talentometerLevelRepository;
			}
		}

		private IRepository<DBMigrationHistory> _dBMigrationHistoryRepository;
		public IRepository<DBMigrationHistory> DBMigrationHistoryRepository
		{
			get
			{
				if (_dBMigrationHistoryRepository == null)
					_dBMigrationHistoryRepository = new TalentifyRepository<DBMigrationHistory>(this.Context);

				return _dBMigrationHistoryRepository;
			}
		}

		public bool ProxyCreationEnabled
		{
			get { return this.Context.Configuration.ProxyCreationEnabled; }
			set { this.Context.Configuration.ProxyCreationEnabled = value; }
		}

		public TalentifyUnitOfWork()
        {
            _context = new TContext();
        }

		public TalentifyUnitOfWork(TContext context)
        {
            _context = context;
        }
	}
}
