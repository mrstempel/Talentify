using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.User
{
	public enum ActionTokenType
	{
		PasswordReset,
		Invite
	}

	public class ActionToken : BaseEntity
	{
		public ActionTokenType Type { get; set; }
		public Guid Token { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime ValidUntil { get; set; }
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
	}

	public class ActionTokenMap : EntityTypeConfiguration<ActionToken>
	{
		public ActionTokenMap()
		{
			// Table Name
			this.ToTable("ActionToken");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime2");
			this.Property(t => t.ValidUntil).HasColumnName("ValidUntil").HasColumnType("datetime2");
		}
	}
}
