using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.FrontendLogic.Library
{
	public enum TimelineItemType
	{
		Message,
		Confirmation,
		Canceled,
		Bonus
	}

	public interface ICoachingRequestTimelineItem
	{
		DateTime CreatedDate { get; }
		int UserId { get; }
		string UserImage { get; }
		string Username { get; }
		string Text { get; }
		TimelineItemType ItemType { get; }
	}
}
