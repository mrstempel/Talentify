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

		public override void Delete(EventRegistration entity)
		{
			if (entity.Bonuspoints > 0)
			{
				//re-add bonuspoints to user
				UnitOfWork.BonuspointRepository.Insert(entity.UserId, entity.Bonuspoints, "Gesetzte Bonuspunkte für Event gutgeschrieben", 0, true, null);
			}
			base.Delete(entity);
		}

		public void SignOff(EventRegistration entity)
		{
			if (entity.Bonuspoints > 0)
			{
				//re-add bonuspoints to user
				UnitOfWork.BonuspointRepository.Insert(entity.UserId, entity.Bonuspoints, "Gesetzte Bonuspunkte für Event gutgeschrieben", 0, true, null);
			}

			entity.CreatedDate = DateTime.Now;
			entity.IsSignedOff = true;
			Update(entity);
		}

		public int GetRegisterCount(int userId)
		{
			var regCount = UnitOfWork.EventRegistrationRepository.AsQueryable().Count(r => r.UserId == userId && !r.IsSignedOff);
			return regCount;
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

		public void MarkAsNotified(int eventId, int userId)
		{
			var reg = AsQueryable().FirstOrDefault(r => r.EventId == eventId && r.UserId == userId);
			if (reg != null)
			{
				reg.WasNotified = true;
				Update(reg);
				//UnitOfWork.Save();
			}
		}

		public void AddComment(int id, string comments)
		{
			var reg = GetById(id);
			reg.Comments = comments;

			Update(reg);
		}

		public int GetNotAttendedCount(int userId)
		{
			return AsQueryable().Count(r => !r.Confirmed && r.HasFollowUpEmail && r.UserId == userId);
		}
	}
}
