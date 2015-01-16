using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Coaching
{
	public enum CoachingTimeDay
	{
		MO,
		DI,
		MI,
		DO,
		FR,
		SA,
		SO
	}

	public class CoachingTime : BaseEntity
	{
		public int UserId { get; set; }
		private Student _user;
		public Student User
		{
			get { return _user; }
			set
			{
				_user = value;
				if (value != null)
					UserId = value.Id;
			}
		}

		public CoachingTimeDay Day { get; set; }
		public string From { get; set; }
		public string To { get; set; }
	}

	public class CoachingTimeMap : EntityTypeConfiguration<CoachingTime>
	{
		public CoachingTimeMap()
		{
			// Table Name
			this.ToTable("CoachingTime");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
