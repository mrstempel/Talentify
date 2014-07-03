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

		public IEnumerable<SearchResultItem> Search(SearchParams searchParams)
		{
			string uploadDir = ConfigurationManager.AppSettings["Upload.Profile"];
			var results = from offer in UnitOfWork.CoachingOfferRepository.AsQueryable()
						join student in UnitOfWork.StudentRepository.AsQueryable() on offer.UserId equals student.Id
						join school in UnitOfWork.SchoolRepository.AsQueryable() on student.SchoolId equals school.Id
						where offer.FromClass <= searchParams.Class &&
						offer.ToClass >= searchParams.Class &&
						offer.SubjectCategoryId == searchParams.SubjectCategoryId
				select new SearchResultItem()
				{
					Id = student.Id,
					Image = student.PictureGuid.HasValue
								? uploadDir + student.PictureGuid.ToString() + "_medium.png"
								: "/Images/tmp-profile-medium.png",
					Name = student.Firstname + " " + student.Surname,
					Address = school.ZipCode + "-" + school.City,
					School = school.Name,
					Comments = offer.Comments
				};

			return results;
		}
	}
}
