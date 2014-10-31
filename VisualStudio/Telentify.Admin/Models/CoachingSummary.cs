using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Talentify.ORM.DAL.Models.Coaching;

namespace Telentify.Admin.Models
{
	public class CoachingSummary
	{
		public CoachingRequest Request { get; set; }
		public string Subject { get; set; }
		public string FromUserName { get; set; }
		public string ToUserName { get; set; }
	}
}