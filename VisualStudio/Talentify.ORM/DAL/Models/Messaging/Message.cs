using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Library;

namespace Talentify.ORM.DAL.Models.Messaging
{
	public class Message : BaseEntity, ICoachingRequestTimelineItem
	{
		public int ConversationId { get; set; }
		private Conversation _conversation;
		public Conversation Conversation
		{
			get { return _conversation; }
			set
			{
				_conversation = value;
				if (value != null)
					ConversationId = value.Id;
			}
		}

		public int FromUserId { get; set; }
		private BaseUser _fromUser;
		public virtual BaseUser FromUser
		{
			get { return _fromUser; }
			set
			{
				_fromUser = value;
				if (value != null)
					FromUserId = value.Id;
			}
		}

		public DateTime CreatedDate { get; set; }
		public string Text { get; set; }
		public virtual ICollection<MessageRecipient> Recipients { get; set; }

		#region ICoachingRequestTimelineItem Implementation

		public int UserId
		{
			get { return FromUserId; }
		}
		
		public string UserImage
		{
			get { return FromUser != null ? FromUser.ProfileImageSmall : null; }
		}

		public string Username
		{
			get { return FromUser != null ? FromUser.Firstname + " " + FromUser.Surname : null; }
		}

		public TimelineItemType ItemType
		{
			get { return TimelineItemType.Message; }
		}

		#endregion
	}

	public class MessageMap : EntityTypeConfiguration<Message>
	{
		public MessageMap()
		{
			// Table Name
			this.ToTable("Message");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime2");
		}
	}
}
