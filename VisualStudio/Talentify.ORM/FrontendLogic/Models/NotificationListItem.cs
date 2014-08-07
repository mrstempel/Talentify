using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Models.Notification;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class NotificationListItem
	{
		public string Image { get; set; }
		public string Link { get; set; }
		public string Text { get; set; }
		public NotificationIconType IconType { get; set; }
		public bool IsNew { get; set; }
		public int Bonus { get; set; }
		public string BadgeIcon { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
