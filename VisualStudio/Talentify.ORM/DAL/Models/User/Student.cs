using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Talentify.ORM.DAL.Models.User
{
	public class Student : BaseUser
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

		public int Class { get; set; }
	}

	public class StudentMap : EntityTypeConfiguration<Student>
	{
		public StudentMap()
		{
			// Table Name
			this.ToTable("Student");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
