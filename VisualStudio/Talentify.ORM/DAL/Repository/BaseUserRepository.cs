using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class BaseUserRepository : TalentifyRepository<BaseUser>
	{
		public BaseUserRepository(TalentifyContext context)
            : base(context)
        {
        }

		public virtual BaseUser GetByEmail(string email)
		{
			return UnitOfWork.BaseUseRepository.Get(u => u.Email == email).FirstOrDefault();
		}

		public virtual bool Register(BaseUser user)
		{
			// set dates
			user.JoinedDate = DateTime.Now;
			// create register code
			user.RegisterCode = Guid.NewGuid();
			// save in database
			Insert(user);
			// send confirmation e-mail
			var mailMsg = new MailMessage(WebConfigurationManager.AppSettings["Email.From"], user.Email);
			mailMsg.Subject = WebConfigurationManager.AppSettings["Email.Register.Subject"];
			mailMsg.Body = string.Format(WebConfigurationManager.AppSettings["Email.Register.Body"], user.RegisterCode);
			Email.Send(mailMsg);

			UnitOfWork.Save();

			return true;
		}
	}
}
