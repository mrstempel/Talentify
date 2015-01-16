using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class TeacherRepository : BaseUserRepository<Teacher>
	{
		public TeacherRepository(TalentifyContext context)
            : base(context)
        {
        }

		public new Teacher GetById(int id)
		{
			return GetById(id, "School,Settings");
		}

		#region Register

		public virtual FormFeedback Register(Teacher teacher, string subjects)
		{
			// check if e-mail is unique
			if (GetByEmail(teacher.Email) != null)
			{
				return new FormFeedback() { IsError = true, Text = "Diese E-Mail Adresse ist bereits vergeben."};
			}
			
			var registerFeedback = new FormFeedback() { IsError = false };
			try
			{
				// set dates
				teacher.JoinedDate = DateTime.Now;
				// create register code
				teacher.RegisterCode = Guid.NewGuid();
				// add subjects
				string[] subjectList = subjects.Split(",".ToCharArray());
				teacher.SubjectCategories = new List<SubjectCategory>();
				foreach (var s in subjectList)
				{
					try
					{
						teacher.SubjectCategories.Add(UnitOfWork.SubjectCategoryRepository.GetById(Convert.ToInt32(s)));
					}
					catch (Exception) {}
				}
				// insert user
				Insert(teacher);
				UnitOfWork.Save();
			}
			catch (Exception ex)
			{
				registerFeedback = new FormFeedback() { IsError = true, Text = ex.Message };
			}

			if (!registerFeedback.IsError)
			{
				// send confirmation e-mail
				var msgBody = new StringBuilder();
				msgBody.Append(string.Format("Vorname: {0}<br/>", teacher.Firstname));
				msgBody.Append(string.Format("Nachname: {0}<br/>", teacher.Surname));
				msgBody.Append(string.Format("E-Mail: {0}<br/>", teacher.Email));
				msgBody.Append(string.Format("Telefon: {0}<br/>", teacher.Phone));
				msgBody.Append(string.Format("Schule: {0}<br/>", UnitOfWork.SchoolRepository.GetById(teacher.SchoolId).Name));
				msgBody.Append("Fächer: ");
				foreach (var s in teacher.SubjectCategories)
				{
					msgBody.Append(s.Name + ", ");
				}
				var lastIndex = msgBody.ToString().LastIndexOf(",");
				var teacherInfo = (lastIndex > 0) ? msgBody.ToString().Substring(0, msgBody.ToString().LastIndexOf(",")) : msgBody.ToString();

				var emailContent =
					string.Format(
						"Vielen Dank für die Anmeldung als talentify Lerncoach. Sie haben sich mit folgenden Daten angemeldet:<br/><br/>{0}<br/><br/>Sie bekommen dieses E-Mail weil Sie dich auf <a href='http://talentify.me' style='color:#0eb48d;'>talentify.me</a> mit Ihrer E-Mailadresse als Lerncoach angemeldet haben. Sollten Sie das nicht gemacht haben oder sich wieder austragen lassen wollen, melden Sie sich bitte einfach unter <a href='mailto:hallo@talentify.at' style='color:#0eb48d;'>hallo@talentify.at</a>",
						teacherInfo);
				Email.SendToTeacher(teacher.Email, WebConfigurationManager.AppSettings["Email.Teacher.Register.Subject"], emailContent);
			}

			return registerFeedback;
		}

		#endregion

		#region Search

		public IEnumerable<SearchResultItem> Search(SearchParams searchParams)
		{
			var teachers = Get(t => t.IsActive);
			if (searchParams.SchoolId != 0)
				teachers = teachers.Where(t => t.SchoolId == searchParams.SchoolId);
			if (searchParams.SubjectCategoryId != 0)
				teachers = teachers.Where(t => t.SubjectCategories.Any(s => s.Id == searchParams.SubjectCategoryId));

			var results = new List<SearchResultItem>();
			if (teachers.Any())
			{
				foreach (var teacher in teachers)
				{
					var resultItem = new SearchResultItem
					{
						Id = teacher.Id,
						Email = teacher.Email,
						Phone = teacher.Phone,
						Name = teacher.Firstname + " " + teacher.Surname,
						Address = teacher.School.ZipCode + "-" + teacher.School.City,
						School = teacher.School.Name
					};
					var comments = string.Empty;
					foreach (var s in teacher.SubjectCategories)
					{
						comments += s.Name + ", ";
					}
					if (comments.Length > 0)
						comments = comments.Substring(0, comments.Length - 2);
					resultItem.Comments = comments;
					results.Add(resultItem);
				}
			}

			return results;
		}

		#endregion
	}
}
