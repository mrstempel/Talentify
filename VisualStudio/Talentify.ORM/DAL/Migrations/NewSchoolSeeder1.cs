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
	public class NewSchoolSeeder1 : BaseSeeder
	{
		public override string Id
		{
			get { return "NewSchoolSeeder1"; }
		}

		public NewSchoolSeeder1(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			addSchools();

			return true;
		}

		private void addSchools()
		{
			var ahs = Unit.SchoolTypeRepository.AsQueryable().FirstOrDefault(s => s.Code == "AHS");
			var htl = Unit.SchoolTypeRepository.AsQueryable().FirstOrDefault(s => s.Code == "HTL");

			// create schools
			Unit.SchoolRepository.Insert(new School()
			{
				Name = "Albertus Magnus Schule",
				Code = "918062",
				Address = "Semperstraße 45",
				ZipCode = "1180",
				City = "Wien", 
				Country = "AT",
				Website = "http://www.ams-wien.at",
				Email = "sekretariat.gym@ams-wien.at",
				Phone = "+43 (0) 1 479691812",
				SchoolType = ahs,
				Latitude = "48.2287753",
				Longitude = "16.3489069"
			});

			Unit.SchoolRepository.Insert(new School()
			{
				Name = "TGM - Die Schule der Technik",
				Code = "920417",
				Address = "Wexstraße 19-23",
				ZipCode = "1200",
				City = "Wien",
				Country = "AT",
				Website = "http://www.tgm.ac.at",
				Email = "info@tgm.ac.at",
				Phone = "+43 (0) 1 33 126-0",
				SchoolType = htl,
				Latitude = "48.2363963",
				Longitude = "16.3695101"
			});
		}
	}
}
