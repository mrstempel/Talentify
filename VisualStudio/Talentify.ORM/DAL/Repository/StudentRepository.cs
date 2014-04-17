using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class StudentRepository : BaseUserRepository
	{
		public StudentRepository(TalentifyContext context)
            : base(context)
        {
        }

		public virtual bool Register(string email, string password, string firstname, string surname, int schoolId)
		{
			// create student
			var student = new Student()
			{
				Email = email,
				Password = PasswordHashing.CalculateSha1(password),
				Firstname = firstname,
				Surname = surname,
				SchoolId = schoolId
			};

			// base register (saves student in database)
			var registerSuccess = Register(student);

			if (registerSuccess)
			{
				// send confirmation e-mail
				var mailMsg = new MailMessage(WebConfigurationManager.AppSettings["Email.From"], student.Email);
				mailMsg.Subject = WebConfigurationManager.AppSettings["Email.Register.Subject"];
				mailMsg.Body = string.Format(WebConfigurationManager.AppSettings["Email.Register.Body"], student.RegisterCode);
				Email.Send(mailMsg);
			}

			return registerSuccess;
		}
	}
}
