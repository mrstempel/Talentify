using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Content;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class EventRegistrationRepository : TalentifyRepository<EventRegistration> 
	{
		public EventRegistrationRepository(TalentifyContext context)
            : base(context)
        {
        }

		public void ConfirmRegistration(int registrationId)
		{
			ConfirmRegistration(GetById(registrationId));
		}

		public void ConfirmRegistration(EventRegistration registration)
		{
			registration.Confirmed = true;
			Update(registration);
			UnitOfWork.Save();
			// set bonuspoints
		}
	}
}
