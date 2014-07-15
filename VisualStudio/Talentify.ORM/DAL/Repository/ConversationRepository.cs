using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
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
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class ConversationRepository : TalentifyRepository<Conversation>
	{
		public ConversationRepository(TalentifyContext context)
            : base(context)
        {
        }

		public Message AddMessage(int conversationId, int fromUserId, int toUserId, int targetId, string text)
		{
			var fromUser = UnitOfWork.BaseUserRepository.GetById(fromUserId);
			var conversation = GetById(conversationId);
			if (conversation.Messages == null)
				conversation.Messages = new List<Message>();

			var message = new Message()
			{
				CreatedDate = DateTime.Now,
				FromUser = fromUser,
				Text = text,
				Recipients = new List<MessageRecipient>()
			};
			message.Recipients.Add(new MessageRecipient() { UserId = toUserId });
			conversation.Messages.Add(message);
			UnitOfWork.ConversationRepository.Update(conversation);
			UnitOfWork.Save();

			// insert notification
			var notifiction = new Notification()
			{
				ToUserId = toUserId,
				SenderId = fromUser.Id,
				TargetId = targetId,
				Text = string.Format("Neue Nachricht von:<br/>{0} {1}", fromUser.Firstname, fromUser.Surname),
				CreatedDate = DateTime.Now,
				SenderType = NotificationSenderType.CoachingRequest,
				IconType = NotificationIconType.None
			};
			UnitOfWork.NotificationRepository.Insert(notifiction);
			UnitOfWork.Save();

			return message;
		}
	}
}
