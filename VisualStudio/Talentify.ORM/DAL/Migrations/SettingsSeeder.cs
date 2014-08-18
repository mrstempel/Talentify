using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Migrations
{
	public class SettingsSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "SettingsSeeder"; }
		}

		public SettingsSeeder(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			addSettings();

			return true;
		}

		private void addSettings()
		{
			var users = Unit.BaseUserRepository.Get();
			foreach (var user in users)
			{
				if (user.Settings == null)
				{
					Unit.BaseUserRepository.AddDefaultSettings(user);
					Unit.BaseUserRepository.Update(user);
				}
			}

			Unit.Save();
		}
	}
}
