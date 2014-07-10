using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.Messaging;
using Talentify.ORM.DAL.Models.Notification;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Library;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class CoachingRequestRepository : TalentifyRepository<CoachingRequest>
	{
		public CoachingRequestRepository(TalentifyContext context)
            : base(context)
        {
        }

		public FormFeedback Insert(CoachingRequest coachingRequest, string message)
		{
			var feedback = new FormFeedback();
			try
			{
				var subjectCategory = UnitOfWork.SubjectCategoryRepository.GetById(coachingRequest.SubjectCategoryId);
				var fromUser = UnitOfWork.BaseUserRepository.GetById(coachingRequest.FromUserId);
				// add initial status
				var initialStatus = new CoachingRequestStatus()
				{
					Text =
						string.Format("Lernanfrage für {0} in Schulstufe {1} gesendet", subjectCategory.Name,
							coachingRequest.Class),
					CreatedDate = DateTime.Now,
					CreatedById = coachingRequest.FromUserId,
					StatusType = StatusType.Request
				};
				coachingRequest.StatusHistory = new List<CoachingRequestStatus>();
				coachingRequest.StatusHistory.Add(initialStatus);

				// create initial message
				var initialMessage = new Message()
				{
					CreatedDate = DateTime.Now,
					Text = message,
					FromUserId = coachingRequest.FromUserId,
					Recipients = new List<MessageRecipient>()
				};
				// add recipient
				initialMessage.Recipients.Add(new MessageRecipient() {UserId = coachingRequest.ToUserId});
				// create conversation
				var conversation = new Conversation()
				{
					ConversationType = ConversationType.CoachingRequest,
					Subject = string.Format("Lernanfrage für {0} in Schulstufe {1}", subjectCategory.Name,
						coachingRequest.Class),
					Messages = new List<Message>()
				};
				conversation.Messages.Add(initialMessage);

				coachingRequest.Conversation = conversation;
				UnitOfWork.CoachingRequestRepository.Insert(coachingRequest);
				UnitOfWork.Save();

				feedback.IsError = false;

				// insert notification
				var notifiction = new Notification()
				{
					ToUserId = coachingRequest.ToUserId,
					SenderId = fromUser.Id,
					Text = string.Format("Lernhilfeanfrage von: {0} {1}", fromUser.Firstname, fromUser.Surname),
					CreatedDate = DateTime.Now,
					SenderType = NotificationSenderType.CoachingRequest,
					IconType = NotificationIconType.None
				};
				UnitOfWork.NotificationRepository.Insert(notifiction);
				UnitOfWork.Save();
			}
			catch (Exception ex)
			{
				feedback.IsError = true;
			}
			
			return feedback;
		}

		public IEnumerable<CoachingRequest> GetByStatus(int toUserId, StatusType statusType)
		{
			var requests = from status in UnitOfWork.CoachingRequestStatusRepository.AsQueryable()
				join coachingRequest in UnitOfWork.CoachingRequestRepository.AsQueryable() on status.CoachingRequestId equals coachingRequest.Id
				join u in UnitOfWork.BaseUserRepository.AsQueryable() on coachingRequest.FromUserId equals u.Id
				where status.StatusType == statusType
				select coachingRequest;

			return requests;
		}

		public CoachingRequestStream GetStream(int coachingRequestId)
		{
			var stream = new CoachingRequestStream() { CoachingRequest  = UnitOfWork.CoachingRequestRepository.GetById(coachingRequestId)};
			var conversation = (from r in UnitOfWork.CoachingRequestRepository.AsQueryable()
				join c in UnitOfWork.ConversationRepository.AsQueryable() on r.Conversation.Id equals c.Id
				where r.Id == coachingRequestId
				select c).FirstOrDefault();

			if (conversation != null)
				stream.TimelineItems = conversation.Messages;

			return stream;
		}
	}
}
