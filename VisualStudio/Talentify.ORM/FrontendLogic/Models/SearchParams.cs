using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class SearchParams
	{
		public int Class { get; set; }
		public int SubjectCategoryId { get; set; }
		public SubjectCategory SubjectCategory { get; set; }
		public int SchoolId { get; set; }
		public School School { get; set; }
		public BaseUser SearchBy { get; set; }
	}
}
