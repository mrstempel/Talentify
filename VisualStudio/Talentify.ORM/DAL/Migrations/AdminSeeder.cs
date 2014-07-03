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
	public class AdminSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "AdminSeeder"; }
		}

		public AdminSeeder(TalentifyContext context)
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
			// add bernhard
			var bernhard = new Admin()
			{
				Email = "bernhard@talentify.at",
				Password = PasswordHashing.CalculateSha1("hofer#talentify!"),
				Firstname = "Bernhard",
				Surname = "Hofer",
				JoinedDate = DateTime.Now
			};
			
			// add david
			var david = new Admin()
			{
				Email = "david@slash.co.at",
				Password = PasswordHashing.CalculateSha1("haifisch"),
				Firstname = "David",
				Surname = "Stempel",
				JoinedDate = DateTime.Now
			};

			Unit.AdminRepository.Insert(bernhard);
			Unit.AdminRepository.Insert(david);
			Unit.Save();
		}
	}
}
