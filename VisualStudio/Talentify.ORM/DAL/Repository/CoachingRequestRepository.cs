using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data.Entity.Core;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using MoreLinq;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
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
				coachingRequest.CreatedDate = DateTime.Now;
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
					Messages = new List<Message>(),
					CreatedDate = DateTime.Now
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
					TargetId = coachingRequest.Id,
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

		public IEnumerable<CoachingRequestListItem> GetByStatus(int toUserId, StatusType statusType)
		{
			var requests = from status in UnitOfWork.CoachingRequestStatusRepository.AsQueryable()
				join coachingRequest in UnitOfWork.CoachingRequestRepository.AsQueryable() on status.CoachingRequestId equals coachingRequest.Id
				join u in UnitOfWork.BaseUserRepository.AsQueryable() on coachingRequest.FromUserId equals u.Id
				where 
					status.StatusType == statusType && 
					(coachingRequest.FromUserId == toUserId || coachingRequest.ToUserId == toUserId)
				orderby coachingRequest.CreatedDate descending 
				select new CoachingRequestListItem()
				{
					RequestId = coachingRequest.Id,
					UsernameFrom = coachingRequest.FromUser.Firstname + " " + coachingRequest.FromUser.Surname,
					EmailFrom = coachingRequest.FromUser.Email,
					PhoneFrom = coachingRequest.FromUser.Phone,
					ImageFrom = coachingRequest.FromUser.PictureGuid,
					UserIdFrom = coachingRequest.FromUser.Id,
					UsernameTo = coachingRequest.ToUser.Firstname + " " + coachingRequest.ToUser.Surname,
					EmailTo = coachingRequest.ToUser.Email,
					PhoneTo = coachingRequest.ToUser.Phone,
					ImageTo = coachingRequest.ToUser.PictureGuid,
					UserIdTo = coachingRequest.ToUser.Id,
					Subject = coachingRequest.SubjectCategory.Name,
					Class = coachingRequest.Class,
					StatusHistoryCount = coachingRequest.StatusHistory.Count()
				};

			var results = requests;
			if (statusType == StatusType.Request)
			{
				results = requests.Where(r => r.StatusHistoryCount == 1);
			}
			if (statusType == StatusType.Appointment)
			{
				results = requests.Where(r => r.StatusHistoryCount == 2);
			}

			return results.DistinctBy(r => r.RequestId);
		}

		public CoachingRequestStream GetStream(int coachingRequestId)
		{
			var stream = new CoachingRequestStream() { CoachingRequest  = UnitOfWork.CoachingRequestRepository.GetById(coachingRequestId)};
			
			var status = stream.CoachingRequest.StatusHistory.LastOrDefault();

			if (status != null)
			{
				if ((status.StatusType == StatusType.Canceled && stream.CoachingRequest.StatusHistory.Any(s => s.StatusType == StatusType.Completed)) ||
					(status.StatusType == StatusType.Completed && stream.CoachingRequest.StatusHistory.Any(s => s.StatusType == StatusType.Canceled)))
				{
					stream.Status = StatusType.Conflicted;
				}
				else
				{
					stream.Status = status.StatusType;	
				}
			}

			var conversation = (from r in UnitOfWork.CoachingRequestRepository.AsQueryable()
				join c in UnitOfWork.ConversationRepository.AsQueryable() on r.Conversation.Id equals c.Id
				where r.Id == coachingRequestId
				select c).FirstOrDefault();

			var timeLineItems = stream.CoachingRequest.StatusHistory.Cast<ICoachingRequestTimelineItem>().Skip(1).ToList();
			
			if (conversation != null)
				timeLineItems.AddRange(conversation.Messages);

			stream.TimelineItems = timeLineItems.OrderBy(i => i.CreatedDate).ToList();

			return stream;
		}

		public bool HasOpenRequest(int fromUserId, int toUserId, int subjectCategoryId)
		{
			var openRequest = (from cr in UnitOfWork.CoachingRequestRepository.AsQueryable()
							   join status in UnitOfWork.CoachingRequestStatusRepository.AsQueryable() on cr.Id equals status.CoachingRequestId
							   where cr.FromUserId == fromUserId && cr.ToUserId == toUserId && cr.SubjectCategoryId == subjectCategoryId
							   orderby status.CreatedDate descending
							   select new
							   {
								   crId = cr.Id,
								   sType = status.StatusType
							   }).FirstOrDefault();

			return (openRequest != null && openRequest.sType != StatusType.Completed && openRequest.sType != StatusType.Canceled);
		}

		public CoachingRequestStatus UpdateStatus(int coachingRequestId, StatusType status, BaseUser fromUser)
		{
			var statusMessage = "hat Lernanfrage bestätigt";
			var notificationMessage = string.Format("Lernhilfeanfrage bestätigt von: {0} {1}", fromUser.Firstname, fromUser.Surname);

			if (status == StatusType.Completed)
			{
				statusMessage = string.Format("hat Lernhilfe bewertet");
				notificationMessage = string.Format("Lernhilfe bewertet von: {0} {1}", fromUser.Firstname, fromUser.Surname);
			}

			return UpdateStatus(coachingRequestId, status, fromUser, statusMessage, notificationMessage);
		}

		public CoachingRequestStatus UpdateStatus(int coachingRequestId, StatusType status, BaseUser fromUser, string statusMessage, string notificationMessage)
		{
			// insert new status
			var newStatus = new CoachingRequestStatus()
			{
				Text = statusMessage,
				CreatedDate = DateTime.Now,
				CreatedById = fromUser.Id,
				StatusType = status
			};
			var coachingRequest = GetById(coachingRequestId);
			coachingRequest.StatusHistory.Add(newStatus);
			Update(coachingRequest);
			UnitOfWork.Save();

			// send notification
			var notifiction = new Notification()
			{
				ToUserId = (coachingRequest.ToUserId == fromUser.Id) ? coachingRequest.FromUserId : coachingRequest.ToUserId,
				SenderId = fromUser.Id,
				TargetId = coachingRequest.Id,
				Text = notificationMessage,
				CreatedDate = DateTime.Now,
				SenderType = NotificationSenderType.CoachingRequest,
				IconType = (status != StatusType.Canceled && status != StatusType.Rejected) ? NotificationIconType.Confirmed : NotificationIconType.Cancelled
			};
			UnitOfWork.NotificationRepository.Insert(notifiction);
			UnitOfWork.Save();

			return newStatus;
		}

		public bool SetCoachingRequestRating(int coachingRequestId, int val1, int val2, int val3, BaseUser fromUser, bool setBonus = true)
		{
			var coachingRequest = GetById(coachingRequestId);

			if (coachingRequest.Ratings == null)
				coachingRequest.Ratings = new List<CoachingRating>();

			var toUserId = coachingRequest.FromUserId == fromUser.Id ? coachingRequest.ToUserId : coachingRequest.FromUserId;

			// check if ratings from this user already exist
			if (coachingRequest.Ratings.Any(r => r.FromUserId == fromUser.Id))
				return false;

			coachingRequest.Ratings.Add(new CoachingRating()
			{
				CoachingRequestId = coachingRequestId,
				FromUserId = fromUser.Id,
				RatingType = RatingType.Helpful,
				ToUserId = toUserId,
				Value = val1
			});

			coachingRequest.Ratings.Add(new CoachingRating()
			{
				CoachingRequestId = coachingRequestId,
				FromUserId = fromUser.Id,
				RatingType = RatingType.OnTime,
				ToUserId = toUserId,
				Value = val2
			});

			coachingRequest.Ratings.Add(new CoachingRating()
			{
				CoachingRequestId = coachingRequestId,
				FromUserId = fromUser.Id,
				RatingType = RatingType.Price,
				ToUserId = toUserId,
				Value = val3
			});

			Update(coachingRequest);
			UnitOfWork.Save();

			if (setBonus)
			{
				var bonusUserId = coachingRequest.FromUserId == fromUser.Id ? coachingRequest.FromUserId : coachingRequest.ToUserId;
				SetCoachingRequestBonus(coachingRequest, bonusUserId);
			}

			return true;
		}

		public bool SetCoachingRequestRating(int coachingRequestId, int val1, int val2, int val3, BaseUser fromUser, DateTime date, int duration)
		{
			var isRatingOkay = SetCoachingRequestRating(coachingRequestId, val1, val2, val3, fromUser, false);
			if (isRatingOkay)
			{
				var coachingRequest = GetById(coachingRequestId);
				coachingRequest.Date = date;
				coachingRequest.Duration = duration;
				Update(coachingRequest);
				UnitOfWork.Save();

				var bonusUserId = coachingRequest.FromUserId == fromUser.Id ? coachingRequest.ToUserId : coachingRequest.FromUserId;
				SetCoachingRequestBonus(coachingRequest, bonusUserId);

				return true;
			}

			return false;
		}

		private void SetCoachingRequestBonus(CoachingRequest coachingRequest, int bonusUserId)
		{
			// check if both ratings are set
			if (coachingRequest.Ratings != null && coachingRequest.Ratings.Count > 3)
			{
				// set bonuspoints
				var bonuspoints = Convert.ToInt32(BonusPointsFor.CoachingOfferCompleted*coachingRequest.Duration);
				if (coachingRequest.Price < 5)
					bonuspoints = bonuspoints*2;

				UnitOfWork.BonuspointRepository.Insert(bonusUserId, bonuspoints, "Bestätigte Lernhilfe");
			}
		}
	}
}
