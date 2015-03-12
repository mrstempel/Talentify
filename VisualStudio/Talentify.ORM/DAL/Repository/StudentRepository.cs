using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic;
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

		public bool SetRegisterCode(Student student, string code)
		{
			var registerCode = UnitOfWork.RegisterCodeRepository.AsQueryable().FirstOrDefault(c => c.Code == code);
			if (registerCode != null && registerCode.UsedDate == null)
			{
				if (student.Id != 0)
				{
					registerCode.UserId = student.Id;
				}
				else
				{
					registerCode.User = student;	
				}
				registerCode.UsedDate = DateTime.Now;
				UnitOfWork.RegisterCodeRepository.Update(registerCode);
				return true;
			}

			return false;
		}

		#region Register

		public virtual FormFeedback Register(Student student, string token, string schoolRegisterCode, string newSchool, string schoolEmail)
		{
			if (newSchool == null)
				newSchool = string.Empty;

			// check if school e-mail is provided, if so - set email
			if (!string.IsNullOrEmpty(schoolEmail) && student.SchoolId.HasValue)
			{
				student.Email = schoolEmail + "@" + UnitOfWork.SchoolRepository.GetById(student.SchoolId.Value).EmailSuffix;
			}
			else
			{
				student.BlockedReason = BlockedReasons.NoSchoolConfirmation.ToString();
			}

			// check if e-mail is unique
			if (string.IsNullOrEmpty(student.Email) || GetByEmail(student.Email) != null)
			{
				return new FormFeedback() { IsError = true, Text = "Diese E-Mail Adresse ist bereits vergeben."};
			}

			var registerFeedback = new FormFeedback() { IsError = false };
			try
			{
				// set dates
				student.JoinedDate = DateTime.Now;
				// create register code
				student.RegisterCode = Guid.NewGuid();
				// set default settings
				if (!student.SettingsId.HasValue && student.Settings == null)
				{
					AddDefaultSettings(student);
				}
				// insert user
				Insert(student);
				// create default subscription
				var subscription = new Subscription()
				{
					Membership = UnitOfWork.MembershipRepository.AsQueryable().FirstOrDefault(m => m.Type == MembershipType.Free),
					User = student,
					PurchaseDate = DateTime.Now
				};
				// insert default subscription
				UnitOfWork.SubscriptionRepository.Insert(subscription);
				// commit to database
				UnitOfWork.Save();
			}
			catch (Exception ex)
			{
				registerFeedback = new FormFeedback() { IsError = true, Text = ex.Message };
			}

			if (!registerFeedback.IsError)
			{
				// send confirmation e-mail
				var confirmUrl = string.Format(ConfigurationManager.AppSettings["BaseUrl"] + "/Register/Confirm?c={0}&t={1}", student.RegisterCode, token);
				var emailContent =
					string.Format(
						"Vielen Dank für die Anmeldung bei talentify. Bitte klicke auf den folgenden Link um die Registrierung abzuschließen:<br/><br/><a href='{0}' style='color:#0eb48d;'>{0}</a><br/><br/>Du bekommst dieses E-Mail weil du dich auf <a href='http://talentify.me' style='color:#0eb48d;'>talentify.me</a> mit deiner E-Mailadresse angemeldet hast. Solltest du das nicht gemacht haben, melde dich bitte unter <a href='mailto:hallo@talentify.at' style='color:#0eb48d;'>hallo@talentify.at</a> und gebe uns Bescheid.",
						confirmUrl);
				
				// different mail text for user with no school
				if (!student.SchoolId.HasValue)
				{
					emailContent = string.Format(
						"Vielen Dank für die Anmeldung bei talentify. Deine Schule {0} ist noch nicht freigeschalten? Kein Problem, sobald wir mind. 10 Anmeldung von deiner Schule haben schalten wir diese frei und geben dir Bescheid. In der Zwischenzeit hast du eingeschränkten Zugriff und kannst Workshops besuchen.<br/><br/>Dazu klicke bitte auf den folgenden Link um die Registrierung abzuschließen:<br/><br/><a href='{1}' style='color:#0eb48d;'>{1}</a><br/><br/>Du bekommst dieses E-Mail weil du dich auf <a href='http://talentify.me' style='color:#0eb48d;'>talentify.me</a> mit deiner E-Mailadresse angemeldet hast. Solltest du das nicht gemacht haben, melde dich bitte unter <a href='mailto:hallo@talentify.at' style='color:#0eb48d;'>hallo@talentify.at</a> und gebe uns Bescheid.",
						newSchool, confirmUrl);
				}

				Email.Send(student.Email, WebConfigurationManager.AppSettings["Email.Register.Subject"], emailContent);
			}

			return registerFeedback;
		}

		public virtual FormFeedback RegisterConfirm(Student student, Guid registerCode, string inviteToken, string password, string confirmOption,
			HttpPostedFileBase upload, string basePath, string schoolRegisterCode)
		{
			// confirm with code
			if (confirmOption == "opt-code")
			{
				if (student.SchoolId.HasValue && !SetRegisterCode(student, schoolRegisterCode))
				{
					return new FormFeedback() {IsError = true, Text = "Der angegebene Registrierungscode ist nicht korrekt."};
				}
				
				student.BlockedReason = null;
			}

			// confirm with upload
			if (confirmOption == "opt-ausweis")
			{
				if (upload.ContentLength > 0)
				{
					// check upload properties
					// filesize < 2mb
					if (upload.ContentLength > 2100000)
					{
						return new FormFeedback() { IsError = true, Text = "Das File ist zu groß. Die maximale Dateigröe beträgt 2 MB." };
					}

					// only png, jpg, jpeg and pdf allowed
					var fileExtension = Path.GetExtension(upload.FileName).ToLower();

					if (fileExtension != ".png" && fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".pdf")
					{
						return new FormFeedback() { IsError = true, Text = "Es sind nur PNG, JPG, JPEG und PDF Dateien erlaubt." };
					}

					student.AusweisGuid = Guid.NewGuid();
					// save original file
					var fileName = student.AusweisGuid.ToString() + fileExtension;
					var originalPath = Path.Combine(basePath, fileName);
					upload.SaveAs(originalPath);
					student.AusweisExtension = fileExtension;

					// save large file
					var filenameLarge = student.AusweisGuid.ToString() + "_optimiert" + fileExtension;
					var pathLarge = Path.Combine(basePath, filenameLarge);
					KwIt.Project.Pattern.Utils.Image.SaveResize(originalPath, pathLarge, 334);

					// delete original file
					File.Delete(originalPath);
				}
				else
				{
					return new FormFeedback() { IsError = true, Text = "Kein gültiger Schülerausweis gefunden." };
				}
			}
			
			// this will be done in the confirmation step
			// set password hash
			student.Password = PasswordHashing.CalculateSha1(password);

			RegisterConfirm(registerCode, inviteToken);
			NewsletterRegistration.Subscribe(student);

			return new FormFeedback() { IsError = false };
		}

		#endregion

		public void Update(Student entity, bool doGetAttackedModel = true)
		{
			if (doGetAttackedModel)
				entity = GetAttachedModel(entity);

			base.Update(entity);

			// update newsletter subscription
			//var settings = UnitOfWork.UserSettingsRepository.AsQueryable().FirstOrDefault(s => s.Id == entity.SettingsId);
			if (entity.Email != null && entity.Settings.HasNewsletter)
			{
				NewsletterRegistration.Subscribe(entity);
			}
		}

		public bool SetSurveyData(int userId, string parentEducation, string hearedOfTalentify)
		{
			try
			{
				var user = GetById(userId);
				user.ParentEducation = parentEducation;
				user.HeardOfTalentify = hearedOfTalentify;
				Update(user, false);
				UnitOfWork.Save();

				UnitOfWork.BonuspointRepository.Insert(userId, BonusPointsFor.Survey, "Teilnahme an Umfrage", user.Id);

				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		#region Delete

		public void DeleteAccount(int userId, string uploadPath)
		{
			// delete all coaching offers from this user
			var coachingOffers = UnitOfWork.CoachingOfferRepository.Get(c => c.UserId == userId);
			//while (coachingOffers.Count() > 0)
			foreach (var offer in coachingOffers)
			{
				// delete offer
				UnitOfWork.CoachingOfferRepository.Delete(offer);
			}

			UnitOfWork.Save();

			var student = GetById(userId);
			var pictureGuid = student.PictureGuid;
			var email = student.Email;
			// reset user data
			student.IsDeleted = true;
			student.Email = null;
			student.Password = null;
			student.Firstname = "Gelöschter";
			student.Surname = "Benutzer";
			student.BirthDate = null;
			student.Address = null;
			student.ZipCode = null;
			student.City = null;
			student.Country = null;
			student.Phone = null;
			student.RegisterCode = null;
			student.PictureGuid = null;
			student.Gender = null;
			// reset student data
			student.School = null;
			student.Class = 0;
			student.AboutMe = null;
			student.IsCoachingEnabled = false;
			student.CoachingPrice = 0;
			Update(student);
			UnitOfWork.Save();
			
			// delete avatar
			if (pictureGuid.HasValue)
			{
				var filenameSmall = pictureGuid.Value.ToString() + "_small.png";
				var filenameMedium = pictureGuid.Value.ToString() + "_medium.png";
				var filenameLarge = pictureGuid.Value.ToString() + "_large.png";

				System.IO.File.Delete(Path.Combine(uploadPath, filenameSmall));
				System.IO.File.Delete(Path.Combine(uploadPath, filenameMedium));
				System.IO.File.Delete(Path.Combine(uploadPath, filenameLarge));
			}

			// unsubscribe from newsletter
			NewsletterRegistration.Unsubscribe(email);

			Email.SendDelete(email, WebConfigurationManager.AppSettings["Email.Delete.Subject"]);
		}

		#endregion

		#region Get

		public IEnumerable<Student> GetFullRegisteredList()
		{
			return AsQueryable().Where(s => s.RegisterCode == null && s.IsDeleted == false);
		}

		public IEnumerable<Student> GetNotRegisterConfirmedList()
		{
			return AsQueryable().Where(s => s.RegisterCode != null && s.IsDeleted == false);
		}

		public IEnumerable<Student> GetAusweisList()
		{
			return AsQueryable().Where(s => s.RegisterCode == null && s.IsDeleted == false && s.BlockedReason == BlockedReasons.NoSchoolConfirmation.ToString() && s.AusweisGuid != null);
		}

		public IEnumerable<Student> GetCoachList()
		{
			return (from s in AsQueryable()
				join coaching in UnitOfWork.CoachingOfferRepository.AsQueryable() on s.Id equals coaching.UserId
					where s.IsCoachingEnabled && s.IsDeleted == false
				select s).Distinct();
		}

		#endregion

		#region Statistics

		public int GetFullRegisteredCount()
		{
			return AsQueryable().Count(s => s.RegisterCode == null && s.IsDeleted == false && s.BlockedReason != BlockedReasons.NoSchoolConfirmation.ToString());
		}

		public int GetSchuelerausweisCount()
		{
			return AsQueryable().Count(s => s.RegisterCode == null && s.IsDeleted == false && s.BlockedReason == BlockedReasons.NoSchoolConfirmation.ToString() && s.AusweisGuid != null);
		}

		public int GetNotRegisterConfirmedCount()
		{
			return AsQueryable().Count(s => s.RegisterCode != null && s.IsDeleted == false);
		}

		public int GetDeletedCount()
		{
			return AsQueryable().Count(s => s.IsDeleted);
		}

		public int GetCoachCount()
		{
			var coaches = (from s in AsQueryable()
				join coaching in UnitOfWork.CoachingOfferRepository.AsQueryable() on s.Id equals coaching.UserId
				where s.IsCoachingEnabled && s.IsDeleted == false
				select s).Distinct();

			return coaches.Count();
		}

		#endregion
	}
}
