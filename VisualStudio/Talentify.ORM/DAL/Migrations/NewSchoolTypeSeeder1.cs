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
	public class NewSchoolTypeSeeder1 : BaseSeeder
	{
		public override string Id
		{
			get { return "NewSchoolTypeSeeder1"; }
		}

		public NewSchoolTypeSeeder1(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			addSchoolTypes();

			return true;
		}

		private void addSchoolTypes()
		{
			var volks = new SchoolType()
			{
				Code = "VS",
				Name = "Volksschule",
				StartClass = 1,
				EndClass = 4
			};
			Unit.SchoolTypeRepository.Insert(volks);

			var mittelschule = new SchoolType()
			{
				Code = "NMS",
				Name = "Neue Mittelschule",
				StartClass = 1,
				EndClass = 4
			};
			Unit.SchoolTypeRepository.Insert(mittelschule);

			var haupt = new SchoolType()
			{
				Code = "HS",
				Name = "Hauptschule",
				StartClass = 1,
				EndClass = 4
			};
			Unit.SchoolTypeRepository.Insert(haupt);

			var poly = new SchoolType()
			{
				Code = "PS",
				Name = "Polytechnische Schule",
				StartClass = 1,
				EndClass = 4
			};
			Unit.SchoolTypeRepository.Insert(poly);

			Unit.Save();
		}
	}
}
