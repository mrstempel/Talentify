using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Context;

namespace Talentify.ORM.DAL.Migrations
{
	public sealed class TalentifyConfiguration : DbMigrationsConfiguration<TalentifyContext>
	{
		public TalentifyConfiguration()
        {
			AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = false;
        }

		protected override void Seed(TalentifyContext context)
		{
			List<BaseSeeder> seeder = new List<BaseSeeder>();
			seeder.Add(new InitialSeeder(context));
			seeder.Add(new AdminSeeder(context));
			seeder.Add(new SchoolLatLngSeeder(context));
			seeder.Add(new BadgeSeeder(context));
			seeder.Add(new TalentometerLevelSeeder(context));
			seeder.Add(new NewSchoolSeeder1(context));
			seeder.Add(new NewSchoolTypeSeeder1(context));
			seeder.Add(new AdminSeeder2(context));
			seeder.Add(new IsActiveSeeder(context));

			foreach (var baseSeeder in seeder)
			{
				if (baseSeeder.ShouldSeed && baseSeeder.Seed())
				{
					baseSeeder.FinishSeed();
				}
			}
		}
	}
}
