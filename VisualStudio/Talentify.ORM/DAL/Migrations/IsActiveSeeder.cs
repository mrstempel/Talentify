using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Migrations
{
	public class IsActiveSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "IsActiveSeeder"; }
		}

		public IsActiveSeeder(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			setIsActive();

			return true;
		}

		private void setIsActive()
		{
			var allusers = Unit.BaseUserRepository.Get(null, null, "Settings,Subscriptions");
			foreach (var baseUser in allusers)
			{
				baseUser.IsActive = true;
				Unit.BaseUserRepository.Update(baseUser);
			}

			Unit.Save();
		}
	}
}
