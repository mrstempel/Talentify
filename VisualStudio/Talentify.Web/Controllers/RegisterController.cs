using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using KwIt.Project.Pattern.DAL.Context;
using Talentify.ORM.DAL.Models.Talentecheck;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.DAL.Repository;
using Talentify.ORM.FrontendLogic;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;
using Talentify.ORM.Utils;
using WebMatrix.WebData;

namespace Talentify.Web.Controllers
{
	[AllowAnonymous]
    public class RegisterController : BaseController
    {
        public ActionResult Index()
        {
			ViewBag.IsTalentecheck = (this.TalentecheckSessionFromCookie != null && this.TalentecheckSessionFromCookie.UserId == 0 && this.TalentecheckSessionFromCookie.IsFinished);
            return View(new Student());
        }

		[HttpPost]
	    public ActionResult Index(Student student)
		{
			ViewBag.IsTalentecheck = false;
			// no school option
			if (student.SchoolId == 0)
			{
				student.SchoolId = null;
			}

			var saveFeedback = UnitOfWork.StudentRepository.Register(student, Request["token"], Request["RegisterCode"], Request["NewSchool"], Request["schoolEmail"]);

			if (saveFeedback.IsError)
				this.FormError = saveFeedback;
			else
			{
				if (!student.SchoolId.HasValue)
				{
					// send mail with school suggestion
					var mailMsg = new MailMessage(new MailAddress(student.Email),
						new MailAddress(ConfigurationManager.AppSettings["Email.Feedback.To"]));
					mailMsg.Subject = "Neuer Schulvorschlag";
					mailMsg.Body = string.Format("{0} {1} (Email: {2}) hat eine neue Schule vorgeschlagen: {3}", student.Firstname, student.SurnameFormatted, student.Email, Request["NewSchool"]);
					Email.Send(mailMsg);
				}

				// if talentecheck register, save user-id to talentecheck-session
				if (this.TalentecheckSessionFromCookie != null && this.TalentecheckSessionFromCookie.UserId == 0 && this.TalentecheckSessionFromCookie.IsFinished)
				{
					this.TalentecheckSessionFromCookie.UserId = student.Id;
					UnitOfWork.TalentecheckSessionRepository.Update(this.TalentecheckSessionFromCookie);
					UnitOfWork.Save();

					if (Request.Cookies["TalentecheckGuid"] != null)
					{
						var myCookie = new HttpCookie("TalentecheckGuid");
						myCookie.Expires = DateTime.Now.AddDays(-1);
						Response.Cookies.Add(myCookie);
					}
				}

				this.FormSuccess = saveFeedback;
			}

			return View(student);
	    }

	    public ActionResult Confirm(string c, string t)
	    {
		    ViewBag.IsTalentecheckRegister = false;

		    Student user = null;
		    try
		    {
			    user = UnitOfWork.StudentRepository.AsQueryable().FirstOrDefault(u => u.RegisterCode == new Guid(c));
			    if (user.SchoolId.HasValue)
			    {
				    user.School = UnitOfWork.SchoolRepository.GetById(user.SchoolId.Value);
			    }
			    else
			    {
				    ViewBag.NoSchool = true;
			    }
		    }
		    catch (Exception ex) { }

			if (user == null)
			{
				this.FormError = new FormFeedback();
			}

		    try
		    {
				ViewBag.IsTalentecheckRegister = UnitOfWork.BaseUserRepository.GetTalentecheckSession(user) != null;
		    }
		    catch (Exception){}
			

			return View(user);
	    }

		[HttpPost]
		public ActionResult Confirm(int userId, string c, string t, HttpPostedFileBase ausweisUpload)
		{
			ViewBag.IsTalentecheck = (this.TalentecheckSessionFromCookie != null && this.TalentecheckSessionFromCookie.UserId == 0 && this.TalentecheckSessionFromCookie.IsFinished);
			var student = UnitOfWork.StudentRepository.GetById(userId);
			try
			{
				var feedback = UnitOfWork.StudentRepository.RegisterConfirm(student, new Guid(c), t, Request["Password"],
					Request["confirmOption"], ausweisUpload,
					Server.MapPath("~" + ConfigurationManager.AppSettings["Upload.Ausweis"]), Request["SchoolRegisterCode"]);

				if (feedback.IsError)
				{
					this.FormError = feedback;
					this.FormError.AutoClose = false;
				}
				else
				{
					if (WebSecurity.Login(student.Email, student.Password))
					{
						Session["IsFirstLogin"] = true;

						if (TalentecheckSession != null)
						{
							var talentecheckBonus = new TalentecheckBonus()
							{
								Action = TalentecheckBonusAction.Register.ToString(),
								Points = TalentecheckBonusPointsFor.Register,
								CreateDate = DateTime.Now,
								TalentecheckSessionId = TalentecheckSession.Id
							};
							UnitOfWork.TalentecheckBonusRepository.Insert(talentecheckBonus);
							UnitOfWork.BadgeRepository.AddBadgeToUser(student, TalentecheckSession.TypMax.ToString());

							var inviteSession =
								UnitOfWork.TalentecheckSessionRepository.AsQueryable()
									.FirstOrDefault(s => s.SessionId == TalentecheckSession.InviteSessionId);
							if (inviteSession != null)
							{
								var inviteBonus = new TalentecheckBonus()
								{
									Action = TalentecheckBonusAction.Invite.ToString(),
									Points = TalentecheckBonusPointsFor.Invite,
									CreateDate = DateTime.Now,
									TalentecheckSessionId = inviteSession.Id
								};
								UnitOfWork.TalentecheckBonusRepository.Insert(inviteBonus);
							}

							UnitOfWork.Save();
						}


						return RedirectToAction("Index", "Start");
					}
				}
			}
			catch (Exception ex)
			{
				this.FormError = new FormFeedback() { AutoClose = false };
			}

			try
			{
				ViewBag.IsTalentecheckRegister = UnitOfWork.BaseUserRepository.GetTalentecheckSession(student) != null;
			}
			catch (Exception) { }

			return View(student);
		}

		public ActionResult Agb()
		{
			return View();
		}

		public ActionResult Teacher()
		{
			return View(new Teacher());
		}

		[HttpPost]
		public ActionResult Teacher(Teacher teacher)
		{
			var saveFeedback = UnitOfWork.TeacherRepository.Register(teacher, Request["CoachingSubjects"]);

			if (saveFeedback.IsError)
				this.FormError = saveFeedback;
			else
				this.FormSuccess = saveFeedback;
			return View(teacher);
		}
    }
}
