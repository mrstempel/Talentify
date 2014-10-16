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
	public class TalentometerLevelSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "TalentometerLevelSeeder"; }
		}

		public TalentometerLevelSeeder(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			addLevels();

			return true;
		}

		private void addLevels()
		{
			// Talent
			var talent = new TalentometerLevel() {Level = 0, MinPoints = 0, Name = "Talent", ConfirmedEvents = 0};
			Unit.TalentometerLevelRepository.Insert(talent);
			// Padawan
			var padawan = new TalentometerLevel() { Level = 1, MinPoints = 200, Name = "Padawan", ConfirmedEvents = 0 };
			Unit.TalentometerLevelRepository.Insert(padawan);
			// Coach
			var coach = new TalentometerLevel() { Level = 2, MinPoints = 400, Name = "Coach", ConfirmedEvents = 0 };
			Unit.TalentometerLevelRepository.Insert(coach);
			// Rockstar
			var rockstar = new TalentometerLevel() { Level = 3, MinPoints = 500, Name = "Rockstar", ConfirmedEvents = 5 };
			Unit.TalentometerLevelRepository.Insert(rockstar);
			// Legendary
			var legendary = new TalentometerLevel() { Level = 4, MinPoints = 750, Name = "Legendary", ConfirmedEvents = 10 };
			Unit.TalentometerLevelRepository.Insert(legendary);
			// Yoda
			var yoda = new TalentometerLevel() { Level = 5, MinPoints = 1000, Name = "Yoda", ConfirmedEvents = 20 };
			Unit.TalentometerLevelRepository.Insert(yoda);

			Unit.Save();
		}
	}
}
