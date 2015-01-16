using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class CoachingTimeRepository : TalentifyRepository<CoachingTime>
	{
		public CoachingTimeRepository(TalentifyContext context)
            : base(context)
        {
        }

		public CoachingTime GetByDay(int userId, CoachingTimeDay day)
		{
			return AsQueryable().FirstOrDefault(i => i.UserId == userId && i.Day == day);
		}

		public void Set(CoachingTime coachingTime)
		{
			Set(coachingTime.UserId, coachingTime.Day, coachingTime.From, coachingTime.To);
		}

		public void Set(int userId, CoachingTimeDay day, string from, string to)
		{
			var coachingTime = AsQueryable().FirstOrDefault(t => t.Day == day && t.UserId == userId);

			if (coachingTime == null)
			{
				coachingTime = new CoachingTime()
				{
					UserId = userId,
					Day = day,
					From = from,
					To = to
				};
				Insert(coachingTime);
			}
			else
			{
				coachingTime.From = from;
				coachingTime.To = to;
				Update(coachingTime);
			}
		}

		public void Remove(int userId, CoachingTimeDay day)
		{
			var coachingTime = AsQueryable().FirstOrDefault(t => t.Day == day && t.UserId == userId);

			if (coachingTime != null)
			{
				Delete(coachingTime);
			}
		}
	}
}
