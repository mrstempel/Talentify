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

		public IEnumerable<StreamItem> GetStream()
		{
			// at this point just load the last 10 registrations
			var lastRegistrations = unitOfWork.StudentRepository.AsQueryable().OrderByDescending(u => u.JoinedDate).Take(10);

			if (lastRegistrations.Any())
			{
				var stream = new List<StreamItem>();
				foreach (var user in lastRegistrations)
				{
					var item = new StreamItem
					{
						Image =
							user.HasProfilePicture
								? string.Format("{0}{1}_medium.png", ConfigurationManager.AppSettings["Upload.Profile"], user.PictureGuid.ToString())
								: "/Images/tmp-profile-medium.png",
						Text = string.Format("{0} {1} ist talentify beigetreten", user.Firstname, user.Surname),
						Link = string.Format("/Profile/Index/{0}", user.Id)
					};
					stream.Add(item);
				}

				return stream;
			}

			return null;
		}
	}
}
