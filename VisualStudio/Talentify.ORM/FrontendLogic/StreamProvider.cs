using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.UnitOfWork;
using Talentify.ORM.FrontendLogic.Models;

namespace Talentify.ORM.FrontendLogic
{
	public class StreamProvider
	{
		private TalentifyUnitOfWork<TalentifyContext> unitOfWork;

		public StreamProvider(TalentifyUnitOfWork<TalentifyContext> unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		public IEnumerable<StreamItem> GetStream(DateTime? lastTime)
		{
			var stream = new List<StreamItem>();

			if (!lastTime.HasValue)
				lastTime = DateTime.MaxValue;

			// load latest registrations
			var lastRegistrations = unitOfWork.StudentRepository.AsQueryable().Where(s => s.JoinedDate < lastTime.Value && s.IsDeleted == false && s.RegisterCode == null).OrderByDescending(u => u.JoinedDate);

			if (lastRegistrations.Any())
			{
				foreach (var user in lastRegistrations)
				{
					var item = new StreamItem
					{
						Image =
							user.HasProfilePicture
								? string.Format("{0}{1}_small.png", ConfigurationManager.AppSettings["Upload.Profile"], user.PictureGuid.ToString())
								: "/Images/default-profile-small.png",
						Text = string.Format("{0} {1} ist talentify beigetreten", user.Firstname, user.SurnameFormatted),
						Link = string.Format("/Profile/Index/{0}", user.Id),
						Time = user.JoinedDate
					};
					stream.Add(item);
				}
			}

			// get latest coaching offers
			var lastCoachingOffers = unitOfWork.CoachingOfferRepository.AsQueryable().Where(c => c.CreatedDate < lastTime.Value).OrderByDescending(c => c.CreatedDate);
			if (lastCoachingOffers.Any())
			{
				foreach (var offer in lastCoachingOffers)
				{
					var item = new StreamItem
					{
						Image =
							offer.User.HasProfilePicture
								? string.Format("{0}{1}_small.png", ConfigurationManager.AppSettings["Upload.Profile"], offer.User.PictureGuid.ToString())
								: "/Images/default-profile-small.png",
						Text = string.Format("{0} {1} bietet jetzt Lernhilfe in {2} an", offer.User.Firstname, offer.User.SurnameFormatted, offer.SubjectCategory.Name),
						Link = string.Format("/Profile/Index/{0}", offer.User.Id),
						Time = offer.CreatedDate
					};
					stream.Add(item);
				}
			}

			if (stream.Any())
			{
				return stream.OrderByDescending(i => i.Time).ToList().Take(5);
			}

			return null;
		}
	}
}
