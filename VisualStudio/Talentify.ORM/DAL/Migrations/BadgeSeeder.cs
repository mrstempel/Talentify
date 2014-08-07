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
	public class BadgeSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "BadgeSeeder"; }
		}

		public BadgeSeeder(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			addBadges();

			return true;
		}

		private void addBadges()
		{
			// greenhorn
			var greenhorn = new Badge()
			{
				Name = "Greenhorn", 
				Description = "Nach erfolgreicher Anmeldung", 
				Icon = "greenhorn"
			};
			Unit.BadgeRepository.Insert(greenhorn);

			// may the 4th be with you
			var starwars = new Badge()
			{
				Name = "May the 4th be with you",
				Description = "Wenn am 4. Mai Lernbegleitung gegeben wurde",
				Icon = "force"
			};
			Unit.BadgeRepository.Insert(starwars);
			
			Unit.Save();
		}
	}
}
