using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.User;

namespace Telentify.Admin.Models
{
	public class StudentWithCoachingOffers
	{
		public Student Student { get; set; }
		public IEnumerable<CoachingOffer> CoachingOffers { get; set; }
	}
}