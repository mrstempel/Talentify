using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Messaging
{
	public class MessageRecipient : BaseEntity
	{
		public int UserId { get; set; }
		private BaseUser _user;
		public BaseUser User
		{
			get { return _user; }
			set
			{
				_user = value;
				if (value != null)
					UserId = value.Id;
			}
		}

		public DateTime? ReadTime { get; set; }
	}

	public class MessageRecipientMap : EntityTypeConfiguration<MessageRecipient>
	{
		public MessageRecipientMap()
		{
			// Table Name
			this.ToTable("MessageRecipient");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.ReadTime).HasColumnName("ReadTime").HasColumnType("datetime2");
		}
	}
}
