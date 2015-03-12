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
	public class SchoolStateSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "SchoolStateSeeder"; }
		}

		public SchoolStateSeeder(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			AddStates();

			return true;
		}

		private void AddStates()
		{
			var allSchools = Unit.SchoolRepository.Get().ToList();
			// 1 - wien
			SetState(allSchools.Where(s => s.ZipCode.StartsWith("1")).ToList(), "WIE");
			// 2, 3 - niederösterreich
			SetState(allSchools.Where(s => s.ZipCode.StartsWith("2") || s.ZipCode.StartsWith("3")).ToList(), "NOE");
 			// 4 - oberösterreich
			SetState(allSchools.Where(s => s.ZipCode.StartsWith("4")).ToList(), "OOE");
			// 5 - salzburg
			SetState(allSchools.Where(s => s.ZipCode.StartsWith("5")).ToList(), "SBG");
			// 6 - tirol
			SetState(allSchools.Where(s => s.ZipCode.StartsWith("6")).ToList(), "TIR");
			// 7 - burgenland
			SetState(allSchools.Where(s => s.ZipCode.StartsWith("7")).ToList(), "BGL");
			// 8 - steiermark
			SetState(allSchools.Where(s => s.ZipCode.StartsWith("8")).ToList(), "STM");
			// 9 - kärnten
			SetState(allSchools.Where(s => s.ZipCode.StartsWith("9")).ToList(), "KAE");
			Unit.Save();
		}

		private void SetState(List<School> schools, string state)
		{
			if (schools.Any())
			{
				foreach (var school in schools)
				{
					school.State = state;
					Unit.SchoolRepository.Update(school);
				}
			}
		}
	}
}
