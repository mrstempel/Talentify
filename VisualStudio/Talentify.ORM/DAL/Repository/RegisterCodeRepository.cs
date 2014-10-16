using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class RegisterCodeRepository : TalentifyRepository<RegisterCode>
	{
		private static Random _random = new Random((int)DateTime.Now.Ticks);

		public RegisterCodeRepository(TalentifyContext context)
            : base(context)
        {
        }

		private string RandomString(int size)
		{
			var builder = new StringBuilder();
			for (int i = 0; i < size; i++)
			{
				char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));
				builder.Append(ch);
			}

			return builder.ToString();
		}

		public void GenerateNewCodes(int schoolId)
		{
			for (int i = 0; i < 100; i++)
			{
				// get 1st random string 
				string rand1 = RandomString(2);
				// get 2nd random string 
				string rand2 = RandomString(3);
				// creat full rand string
				string code = rand1 + rand2;

				var registerCode = new RegisterCode() {Code = code, SchoolId = schoolId};
				Insert(registerCode);
			}
		}
	}
}
