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
	public class EventTimeSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "EventTimeSeeder"; }
		}

		public EventTimeSeeder(TalentifyContext context)
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
			var events = Unit.EventRepository.Get();
			foreach (var e in events)
			{
				var hours = Convert.ToInt16(e.BeginTime.Substring(0, 2));
				var minutes = Convert.ToInt16(e.BeginTime.Substring(3, 2));
				e.BeginDate = e.BeginDate.AddHours(hours).AddMinutes(minutes);
				Unit.EventRepository.Update(e);
			}

			Unit.Save();
		}
	}
}
