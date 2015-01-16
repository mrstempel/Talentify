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
	public class AdminSeeder2 : BaseSeeder
	{
		public override string Id
		{
			get { return "AdminSeeder2"; }
		}

		public AdminSeeder2(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			addAdmins();

			return true;
		}

		private void addAdmins()
		{
			// add doris
			var doris = new Admin()
			{
				Email = "doris@talentify.at",
				Password = PasswordHashing.CalculateSha1("doris#talentify!"),
				Firstname = "Doris",
				Surname = "Hofer",
				JoinedDate = DateTime.Now
			};

			// add Clarissa
			var clarissa = new Admin()
			{
				Email = "clarissa.boeck@gmail.com",
				Password = PasswordHashing.CalculateSha1("clarissa#talentify!"),
				Firstname = "Clarissa",
				Surname = "Böck",
				JoinedDate = DateTime.Now
			};

			// add Veronica
			var veronica = new Admin()
			{
				Email = "veronica.berne@app.or.at",
				Password = PasswordHashing.CalculateSha1("veronica#talentify!"),
				Firstname = "Veronica",
				Surname = "Berne",
				JoinedDate = DateTime.Now
			};

			Unit.AdminRepository.Insert(doris);
			Unit.AdminRepository.Insert(clarissa);
			Unit.AdminRepository.Insert(veronica);
			Unit.Save();
		}
	}
}
