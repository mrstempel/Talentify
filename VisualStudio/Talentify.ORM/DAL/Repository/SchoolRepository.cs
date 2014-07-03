using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Content;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class SchoolRepository : TalentifyRepository<School>
	{
		public SchoolRepository(TalentifyContext context)
            : base(context)
        {
        }

		public new School GetById(int id)
		{
			return GetById(id, "SchoolType");
		}

		public IEnumerable<int> GetClasses(int schoolId)
		{
			var school = GetById(schoolId);
			var classes = new List<int>();
			for (int i = school.SchoolType.StartClass; i <= school.SchoolType.EndClass; i++)
			{
				classes.Add(i);
			}

			return classes;
		}
	}
}
