using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.User
{
	public class RegisterCode : BaseEntity
	{
		public string Code { get; set; }
		public DateTime? UsedDate { get; set; }

		public int? SchoolId { get; set; }
		private School.School _school;
		public School.School School
		{
			get { return _school; }
			set
			{
				_school = value;
				if (value != null)
					SchoolId = value.Id;
			}
		}

		public int? UserId { get; set; }
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

	public class RegisterCodeMap : EntityTypeConfiguration<RegisterCode>
	{
		public RegisterCodeMap()
		{
			// Table Name
			this.ToTable("RegisterCode");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.UsedDate).HasColumnName("UsedDate").HasColumnType("datetime2");
		}
	}
}
