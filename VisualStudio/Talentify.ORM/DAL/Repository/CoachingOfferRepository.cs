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
	public class CoachingOfferRepository : TalentifyRepository<CoachingOffer>
	{
		public CoachingOfferRepository(TalentifyContext context)
            : base(context)
        {
        }

		public override void Insert(CoachingOffer entity)
		{
			// check if this is the first coaching offer
			var bonuspoints = (AsQueryable().Any(c => c.UserId == entity.UserId))
				? BonusPointsFor.CoachingOffer
				: BonusPointsFor.InitialCoachingOffer;

			// check if bonuspoints where already given for this subject
			var notifactionWithSubject =
				UnitOfWork.NotificationRepository.AsQueryable()
					.FirstOrDefault(
						n =>
							n.ToUserId == entity.UserId && n.Text == "Lernhilfe hinzugefügt" &&
							n.AdditionalInfo == entity.SubjectCategoryId.ToString());
			
			if (notifactionWithSubject == null)
			{
				// add bonuspoints
				UnitOfWork.BonuspointRepository.Insert(entity.UserId, bonuspoints, "Lernhilfe hinzugefügt",0, true, entity.SubjectCategoryId.ToString());
			}

			base.Insert(entity);
		}

		public IEnumerable<SearchResultItem> Search(SearchParams searchParams)
		{
			string uploadDir = ConfigurationManager.AppSettings["Upload.Profile"];
			var results = from offer in UnitOfWork.CoachingOfferRepository.AsQueryable()
						join student in UnitOfWork.StudentRepository.AsQueryable() on offer.UserId equals student.Id
						join school in UnitOfWork.SchoolRepository.AsQueryable() on student.SchoolId equals school.Id
						where 
							student.IsCoachingEnabled &&
							offer.FromClass <= searchParams.Class &&
							offer.ToClass >= searchParams.Class &&
							offer.SubjectCategoryId == searchParams.SubjectCategoryId &&
							offer.UserId != searchParams.SearchBy.Id
				select new SearchResultItem()
				{
					Id = student.Id,
					Image = student.PictureGuid.HasValue
								? uploadDir + student.PictureGuid.ToString() + "_medium.png"
								: "/Images/default-profile-medium.png",
					Name = student.Firstname + " " + student.Surname,
					Address = school.ZipCode + "-" + school.City,
					School = school.Name,
					Comments = offer.Comments
				};

			return results;
		}
	}
}
