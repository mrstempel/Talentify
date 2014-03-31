﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Context;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Context
{
	public class TalentifyContext : BaseContext
	{
		// schools
		public DbSet<SchoolType> SchoolTypes { get; set; }
		public DbSet<School> Schools { get; set; }

		// coaching
		public DbSet<SubjectCategory> SubjectCategories { get; set; }

		// users
		public DbSet<BaseUser> BasUsers { get; set; }
		public DbSet<Student> Students { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

			// schools
			modelBuilder.Configurations.Add(new SchoolTypeMap());
			modelBuilder.Configurations.Add(new SchoolMap());

			// coaching
			modelBuilder.Configurations.Add(new SubjectCategoryMap());

			// users
			modelBuilder.Configurations.Add(new BaseUserMap());
			modelBuilder.Configurations.Add(new StudentMap());

			base.OnModelCreating(modelBuilder);
		}
	}
}
