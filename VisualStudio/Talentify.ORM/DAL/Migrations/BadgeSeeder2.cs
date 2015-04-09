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
	public class BadgeSeeder2 : BaseSeeder
	{
		public override string Id
		{
			get { return "BadgeSeeder2"; }
		}

		public BadgeSeeder2(TalentifyContext context)
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
			// Sport
			var sport = new Badge()
			{
				Name = "Sportskanone", 
				Description = "Talentecheck Ergebnis", 
				Icon = "sport"
			};
			Unit.BadgeRepository.Insert(sport);

			// Creative
			var creative = new Badge()
			{
				Name = "Der kreative Kopf",
				Description = "Talentecheck Ergebnis",
				Icon = "creative"
			};
			Unit.BadgeRepository.Insert(creative);

			// Rockstar
			var rockstar = new Badge()
			{
				Name = "Rockstar",
				Description = "Talentecheck Ergebnis",
				Icon = "rockstar"
			};
			Unit.BadgeRepository.Insert(rockstar);

			// Nerd
			var nerd = new Badge()
			{
				Name = "Nerd",
				Description = "Talentecheck Ergebnis",
				Icon = "nerd"
			};
			Unit.BadgeRepository.Insert(nerd);

			// Weltverbesserer
			var weltverbesserer = new Badge()
			{
				Name = "Weltverbesserer",
				Description = "Talentecheck Ergebnis",
				Icon = "weltverbesserer"
			};
			Unit.BadgeRepository.Insert(weltverbesserer);

			// Kommunikator
			var kommunikator = new Badge()
			{
				Name = "Kommunikator",
				Description = "Talentecheck Ergebnis",
				Icon = "kommunikator"
			};
			Unit.BadgeRepository.Insert(kommunikator);

			// Fashionista
			var fashionista = new Badge()
			{
				Name = "Fashionista",
				Description = "Talentecheck Ergebnis",
				Icon = "fashionista"
			};
			Unit.BadgeRepository.Insert(fashionista);

			// Brain
			var brain = new Badge()
			{
				Name = "Brain",
				Description = "Talentecheck Ergebnis",
				Icon = "brain"
			};
			Unit.BadgeRepository.Insert(brain);

			Unit.Save();
		}
	}
}
