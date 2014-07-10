using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.FrontendLogic.Library;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class CoachingRequestStream
	{
		public CoachingRequest CoachingRequest { get; set; }
		public IEnumerable<ICoachingRequestTimelineItem> TimelineItems { get; set; }
	}
}
