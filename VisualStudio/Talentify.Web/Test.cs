using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Messaging;

namespace Talentify.Web
{
	public class Test
	{
		public Test()
		{
			// create request
			var request = new CoachingRequest() {FromUserId = 1, ToUserId = 2, SubjectCategoryId = 1, Class = 2, Price = 8};
			// add initial status
			request.StatusHistory = new List<CoachingRequestStatus>();
			request.StatusHistory.Add(
				new CoachingRequestStatus()
				{
					CreatedById = 1,
					CreatedDate = DateTime.Now,
					CoachingRequest = request,
					StatusType = StatusType.Request,
					Text = "Neue Anfrage für blabla von blabla"
				}
			);

			// start conversation
			request.Conversation = new Conversation();
			request.Conversation.ConversationType = ConversationType.CoachingRequest;
			request.Conversation.Subject = "Lernhilfeanfrage für klasse/fach";
			request.Conversation.Messages = new List<Message>();
			var message = new Message();
			message.Recipients = new List<MessageRecipient>();
			message.Recipients.Add(new MessageRecipient() { UserId = 2 });
			request.Conversation.Messages.Add(message);

			//request.Conversation.Messages




			// add rating
			var rating = new CoachingRating() {FromUserId = 1, ToUserId = 2, RatingType = RatingType.Helpful, Value = 1};
			request.Ratings.Add(rating);
		}
	}
}