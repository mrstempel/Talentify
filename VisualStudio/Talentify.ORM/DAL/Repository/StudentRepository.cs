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
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class StudentRepository : BaseUserRepository<Student>
	{
		public StudentRepository(TalentifyContext context)
            : base(context)
        {
        }

		public new Student GetById(int id)
		{
			return GetById(id, "School,Settings") as Student;
		}

		public override Student GetAttachedModel(Student entity)
		{
			var dbEntry = UnitOfWork.StudentRepository.GetById(entity.Id);
			entity.JoinedDate = dbEntry.JoinedDate;
			entity.PictureGuid = dbEntry.PictureGuid;
			entity.IsPictureLandscape = dbEntry.IsPictureLandscape;
			entity.Password = dbEntry.Password;
			entity.IsCoachingEnabled = dbEntry.IsCoachingEnabled;
			entity.CoachingPrice = dbEntry.CoachingPrice;

			UnitOfWork.StudentRepository.Detach(dbEntry);

			return entity;
		}

		#region Register

		public virtual FormFeedback Register(Student student)
		{
			// check if e-mail is unique
			if (GetByEmail(student.Email) != null)
			{
				return new FormFeedback() { IsError = true, Text = "Diese E-Mail Adresse ist bereits vergeben."};
			}

			// set password hash
			student.Password = PasswordHashing.CalculateSha1(student.Password);
			// base register (saves student in database)
			var registerFeedback = base.Register(student);

			if (!registerFeedback.IsError)
			{
				// send confirmation e-mail
				var mailMsg = new MailMessage(WebConfigurationManager.AppSettings["Email.From"], student.Email);
				mailMsg.Subject = WebConfigurationManager.AppSettings["Email.Register.Subject"];
				mailMsg.Body = string.Format(WebConfigurationManager.AppSettings["Email.Register.Body"], student.RegisterCode);
				Email.Send(mailMsg);
			}

			return registerFeedback;
		}

		#endregion

		public void Update(Student entity, bool doGetAttackedModel = true)
		{
			if (doGetAttackedModel)
				entity = GetAttachedModel(entity);

			base.Update(entity);
		}
	}
}
