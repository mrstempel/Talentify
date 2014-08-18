using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class CoachingRequestListItem
	{
		public int RequestId { get; set; }
		public string UsernameFrom { get; set; }
		public string EmailFrom { get; set; }
		public string PhoneFrom { get; set; }
		public Guid? ImageFrom { get; set; }
		public int UserIdFrom { get; set; }
		public string UsernameTo { get; set; }
		public string EmailTo { get; set; }
		public string PhoneTo { get; set; }
		public Guid? ImageTo { get; set; }
		public int UserIdTo { get; set; }
		public string Subject { get; set; }
		public int Class { get; set; }
		public int StatusHistoryCount { get; set; }
	}
}
