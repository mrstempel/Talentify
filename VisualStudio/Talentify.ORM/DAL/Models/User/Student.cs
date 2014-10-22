using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Models.User
{
	public class Student : BaseUser
	{
		[ProfileComplete("Schule")]
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

		[ProfileComplete("Schulstufe")]
		public int Class { get; set; }
		[ProfileComplete("Lernmotto")]
		public string AboutMe { get; set; }
		public bool IsCoachingEnabled { get; set; }
		public decimal CoachingPrice { get; set; }
		[ProfileComplete("ParentEducation")]
		public string ParentEducation { get; set; }
		public string HeardOfTalentify { get; set; }
		public string FacebookUrl { get; set; }
		public string GooglePlusUrl { get; set; }
		public string TwitterUrl { get; set; }
		public string PinterestUrl { get; set; }
		public string InstagramUrl { get; set; }
		public bool IsParentAccount { get; set; }
		public string FirstnameChild { get; set; }

		public override string Surname
		{
			get { return IsParentAccount ? base.Surname + " (Eltern)" : base.Surname; }
			set { base.Surname = value; }
		}

		public bool HasSchool
		{
			get { return SchoolId.HasValue; }
		}
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
