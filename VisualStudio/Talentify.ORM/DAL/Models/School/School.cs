﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.School
{
	public enum SchoolState
	{
		WIE,
		NOE,
		OOE,
		SBG,
		TIR,
		BGL,
		STM,
		KAE
	}

	public class School : BaseEntity
	{
		public int SchoolTypeId { get; set; }
		private SchoolType _schoolType;
		public SchoolType SchoolType
		{
			get { return _schoolType; }
			set
			{
				_schoolType = value;
				if (value != null)
					SchoolTypeId = value.Id;
			}
		}

		public string Name { get; set; }
		public string Code { get; set; }
		public string Address { get; set; }
		public string ZipCode { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string Website { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Longitude { get; set; }
		public string Latitude { get; set; }
		public string EmailSuffix { get; set; }
		public bool IsActive { get; set; }
	}

	public class SchoolMap : EntityTypeConfiguration<School>
	{
		public SchoolMap()
		{
			// Table Name
			this.ToTable("School");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
