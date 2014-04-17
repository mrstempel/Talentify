using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.Coaching;

namespace Talentify.ORM.DAL.Models.User
{
	public class Teacher : BaseUser
	{
		public int SchoolId { get; set; }
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

		public ICollection<SubjectCategory> SubjectCategories { get; set; } 
	}

	public class TeacherMap : EntityTypeConfiguration<Teacher>
	{
		public TeacherMap()
		{
			// Table Name
			this.ToTable("Teacher");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
