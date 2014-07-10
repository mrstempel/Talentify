﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Models.Coaching;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class SearchParams
	{
		public int Class { get; set; }
		public int SubjectCategoryId { get; set; }
		public SubjectCategory SubjectCategory { get; set; }
	}
}
