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
using Talentify.ORM.DAL.Models.Feedback;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.Messaging;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.Tracking;
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
				return _schoolTypeRepository ?? 
					(_schoolTypeRepository = new TalentifyRepository<SchoolType>(this.Context));
			}
		}

		private SchoolRepository _schoolRepository;
		public SchoolRepository SchoolRepository
		{
			get
			{
				return _schoolRepository ?? 
					(_schoolRepository = new SchoolRepository(this.Context));
			}
		}

		private IRepository<SubjectCategory> _subjectCategoryRepository;
		public IRepository<SubjectCategory> SubjectCategoryRepository
		{
			get 
			{
				return _subjectCategoryRepository ??
				       (_subjectCategoryRepository = new TalentifyRepository<SubjectCategory>(this.Context));
			}
		}

		private CoachingOfferRepository _coachingOfferRepository;
		public CoachingOfferRepository CoachingOfferRepository
		{
			get
			{
				return _coachingOfferRepository ?? 
					(_coachingOfferRepository = new CoachingOfferRepository(this.Context));
			}
		}

		private CoachingRequestRepository _coachingRequestRepository;
		public CoachingRequestRepository CoachingRequestRepository
		{
			get 
			{
				return _coachingRequestRepository ?? 
					(_coachingRequestRepository = new CoachingRequestRepository(this.Context));
			}
		}

		private IRepository<CoachingRequestStatus> _coachingRequestStatusRepository;
		public IRepository<CoachingRequestStatus> CoachingRequestStatusRepository
		{
			get 
			{
				return _coachingRequestStatusRepository ??
				       (_coachingRequestStatusRepository = new TalentifyRepository<CoachingRequestStatus>(this.Context));
			}
		}

		private CoachingTimeRepository _coachingTimeRepository;
		public CoachingTimeRepository CoachingTimeRepository
		{
			get
			{
				return _coachingTimeRepository ??
					(_coachingTimeRepository = new CoachingTimeRepository(this.Context));
			}
		}

		private BaseUserRepository<BaseUser> _baseUserRepository;
		public BaseUserRepository<BaseUser> BaseUserRepository
		{
			get
			{
				return _baseUserRepository ?? 
					(_baseUserRepository = new BaseUserRepository<BaseUser>(this.Context));
			}
		}

		private StudentRepository _studentRepository;
		public StudentRepository StudentRepository
		{
			get
			{
				return _studentRepository ?? 
					(_studentRepository = new StudentRepository(this.Context));
			}
		}

		private TeacherRepository _teacherRepository;
		public TeacherRepository TeacherRepository
		{
			get
			{
				return _teacherRepository ?? 
					(_teacherRepository = new TeacherRepository(this.Context));
			}
		}

		private BaseUserRepository<Admin> _adminRepository;
		public BaseUserRepository<Admin> AdminRepository
		{
			get
			{
				return _adminRepository ?? 
					(_adminRepository = new BaseUserRepository<Admin>(this.Context));
			}
		}

		private ActionTokenRepository _actionTokenRepository;
		public ActionTokenRepository ActionTokenRepository
		{
			get
			{
				return _actionTokenRepository ?? 
					(_actionTokenRepository = new ActionTokenRepository(this.Context));
			}
		}

		private IRepository<UserSettings> _userSettingsRepository;
		public IRepository<UserSettings> UserSettingsRepository
		{
			get 
			{
				return _userSettingsRepository ?? 
					(_userSettingsRepository = new TalentifyRepository<UserSettings>(this.Context));
			}
		}

		private RegisterCodeRepository _registerCodeRepository;
		public RegisterCodeRepository RegisterCodeRepository
		{
			get
			{
				return _registerCodeRepository ?? 
					(_registerCodeRepository = new RegisterCodeRepository(this.Context));
			}
		}

		private IRepository<Talentify.ORM.DAL.Models.Membership.Membership> _membershipRepository;
		public IRepository<Talentify.ORM.DAL.Models.Membership.Membership> MembershipRepository
		{
			get 
			{
				return _membershipRepository ??
				       (_membershipRepository =
					       new TalentifyRepository<Talentify.ORM.DAL.Models.Membership.Membership>(this.Context));
			}
		}

		private IRepository<Subscription> _subscriptionRepository;
		public IRepository<Subscription> SubscriptionRepository
		{
			get 
			{
				return _subscriptionRepository ?? 
					(_subscriptionRepository = new TalentifyRepository<Subscription>(this.Context));
			}
		}

		private BasePageRepository<BasePage> _basePageRepository;
		public BasePageRepository<BasePage> BasePageRepository
		{
			get
			{
				return _basePageRepository ?? 
					(_basePageRepository = new BasePageRepository<BasePage>(this.Context));
			}
		}

		private EventRepository _eventRepository;
		public EventRepository EventRepository
		{
			get
			{
				return _eventRepository ?? 
					(_eventRepository = new EventRepository(this.Context));
			}
		}

		private EventRegistrationRepository _eventRegistrationRepository;
		public EventRegistrationRepository EventRegistrationRepository
		{
			get 
			{
				return _eventRegistrationRepository ??
					(_eventRegistrationRepository = new EventRegistrationRepository(this.Context));
			}
		}

		private ConversationRepository _conversationRepository;
		public ConversationRepository ConversationRepository
		{
			get
			{
				return _conversationRepository ?? 
					(_conversationRepository = new ConversationRepository(this.Context));
			}
		}

		private IRepository<Message> _messageRepository;
		public IRepository<Message> MessageRepository
		{
			get
			{
				return _messageRepository ?? 
					(_messageRepository = new TalentifyRepository<Message>(this.Context));
			}
		}

		private IRepository<MessageRecipient> _messageRecipientRepository;
		public IRepository<MessageRecipient> MessageRecipientRepository
		{
			get 
			{
				return _messageRecipientRepository ??
					(_messageRecipientRepository = new TalentifyRepository<MessageRecipient>(this.Context));
			}
		}

		private NotificationRepository _notificationRepository;
		public NotificationRepository NotificationRepository
		{
			get
			{
				return _notificationRepository ?? 
					(_notificationRepository = new NotificationRepository(this.Context));
			}
		}

		private BonusPointRepository _bonuspointRepository;
		public BonusPointRepository BonuspointRepository
		{
			get
			{
				return _bonuspointRepository ?? 
					(_bonuspointRepository = new BonusPointRepository(this.Context));
			}
		}

		private BadgeRepository _badgeRepository;
		public BadgeRepository BadgeRepository
		{
			get
			{
				return _badgeRepository ?? 
					(_badgeRepository = new BadgeRepository(this.Context));
			}
		}

		private TalentometerLevelRepository _talentometerLevelRepository;
		public TalentometerLevelRepository TalentometerLevelRepository
		{
			get 
			{
				return _talentometerLevelRepository ??
					(_talentometerLevelRepository = new TalentometerLevelRepository(this.Context));
			}
		}

		// tracking
		private IRepository<TrackingClick> _trackingClickRepository;
		public IRepository<TrackingClick> TrackingClickRepository
		{
			get 
			{
				return _trackingClickRepository ?? 
					(_trackingClickRepository = new TalentifyRepository<TrackingClick>(this.Context));
			}
		}

		// feedback
		private EventFeedbackRepository _eventFeedbackRepository;
		public EventFeedbackRepository EventFeedbackRepository
		{
			get
			{
				return _eventFeedbackRepository ??
					(_eventFeedbackRepository = new EventFeedbackRepository(this.Context));
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
