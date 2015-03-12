using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Feedback;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class EventFeedbackRepository : TalentifyRepository<EventFeedback>
	{

		public EventFeedbackRepository(TalentifyContext context)
            : base(context)
        {
        }

		public override void Insert(EventFeedback entity)
		{
			// set bonuspoints
			UnitOfWork.BonuspointRepository.Insert(entity.UserId, BonusPointsFor.EventFeedback, "Workshop Feedback abgegeben", 0, false);
			base.Insert(entity);
		}
	}
}
