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
	public class NewSchoolTypeSeeder2 : BaseSeeder
	{
		public override string Id
		{
			get { return "NewSchoolTypeSeeder2"; }
		}

		public NewSchoolTypeSeeder2(TalentifyContext context)
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
			var asSchule = new SchoolType()
			{
				Code = "AS",
				Name = "Allgemeine Sonderschule bzw. Sonderpädagogisches Zentrum",
				StartClass = 1,
				EndClass = 4
			};
			Unit.SchoolTypeRepository.Insert(asSchule);

			var fsSchule = new SchoolType()
			{
				Code = "FS",
				Name = "Fachschule",
				StartClass = 1,
				EndClass = 3
			};
			Unit.SchoolTypeRepository.Insert(fsSchule);

			var bsSchule = new SchoolType()
			{
				Code = "BS",
				Name = "Berufsschule",
				StartClass = 1,
				EndClass = 3
			};
			Unit.SchoolTypeRepository.Insert(bsSchule);

			Unit.Save();
		}
	}
}
