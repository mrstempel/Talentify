using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.Messaging
{
	public enum ConversationType
	{
		CoachingRequest,
		System
	}

	public class Conversation : BaseEntity
	{
		public string Subject { get; set; }
		public ConversationType ConversationType { get; set; }
		public virtual ICollection<Message> Messages { get; set; } 
	}

	public class ConversationMap : EntityTypeConfiguration<Conversation>
	{
		public ConversationMap()
		{
			// Table Name
			this.ToTable("Conversation");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
