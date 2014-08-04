using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Models.School;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class SchoolInfo
	{
		public School School { get; set; }
		public int CoachingStudentCount { get; set; }
		public int CoachingSubjectCount { get; set; }
	}
}
