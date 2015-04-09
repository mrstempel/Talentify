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
using Talentify.ORM.DAL.Models.Talentecheck;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class BaseUserRepository<TEntity> : TalentifyRepository<TEntity> where TEntity : BaseUser 
	{
		public BaseUserRepository(TalentifyContext context)
            : base(context)
        {
        }

		#region Get User

		public virtual BaseUser GetUserFromLogin(string email)
		{
			return UnitOfWork.BaseUserRepository.Get(u => u.Email == email && u.RegisterCode == null, null, "Settings").FirstOrDefault();
		}

		public virtual BaseUser GetByEmail(string email)
		{
			return UnitOfWork.BaseUserRepository.Get(u => u.Email == email).FirstOrDefault();
		}

		#endregion

		public void AddDefaultSettings(TEntity user)
		{
			user.Settings = new UserSettings() { HasNewsletter = true, HasNotifications = true };
		}

		#region Register

		public virtual BaseUser RegisterConfirm(Guid registerCode, string inviteToken)
		{
			var user = UnitOfWork.BaseUserRepository.AsQueryable().FirstOrDefault(u => u.RegisterCode == registerCode);
			
			if (user != null)
			{
				user.RegisterCode = null;
				user.IsActive = true;
				UnitOfWork.Save();

				// add greenhorn badge
				UnitOfWork.BadgeRepository.AddBadgeToUser(user, "Greenhorn");

				// add bonuspoints for registration
				UnitOfWork.BonuspointRepository.Insert(user.Id, BonusPointsFor.Register, "Erfolgreiche Registrierung");

				// check if user was invited
				if (!string.IsNullOrEmpty(inviteToken))
				{
					var actionToken = UnitOfWork.ActionTokenRepository.AsQueryable().FirstOrDefault(t => t.Token.ToString() == inviteToken);
					if (actionToken != null)
					{
						UnitOfWork.BonuspointRepository.Insert(actionToken.UserId, BonusPointsFor.Invite, "Erfolgreiche Einladung: " + user.Email, user.Id);
					}
				}

				return user;
			}

			return null;
		}

		#endregion

		#region Update

		public virtual void SetProfilePicture(TEntity user, HttpPostedFileBase upload, string basePath)
		{
			// create new picture guid if no profile pictute exists
			if (!user.HasProfilePicture)
			{
				user.PictureGuid = Guid.NewGuid();
			}

			// save original file
			var fileExtension = Path.GetExtension(upload.FileName);
			var fileName = user.PictureGuid.ToString() + fileExtension;
			var originalPath = Path.Combine(basePath, fileName);
			upload.SaveAs(originalPath);
			
			// set landscape flag
			user.IsPictureLandscape = KwIt.Project.Pattern.Utils.Image.IsPictureLandscape(originalPath);
			
			// save small file
			var filenameSmall = user.PictureGuid.ToString() + "_small.png";
			var pathSmall = Path.Combine(basePath, filenameSmall);
			KwIt.Project.Pattern.Utils.Image.SaveResize(originalPath, pathSmall, 35);
			
			// save medium file
			var filenameMedium = user.PictureGuid.ToString() + "_medium.png";
			var pathMedium = Path.Combine(basePath, filenameMedium);
			KwIt.Project.Pattern.Utils.Image.SaveResize(originalPath, pathMedium, 105);
			
			// save large file
			var filenameLarge = user.PictureGuid.ToString() + "_large.png";
			var pathLarge = Path.Combine(basePath, filenameLarge);
			KwIt.Project.Pattern.Utils.Image.SaveResize(originalPath, pathLarge, 334);
			
			// delete original file
			File.Delete(originalPath);
			
			// commit to db
			Update(user);
			UnitOfWork.Save();
		}

		public bool UpdatePassword(TEntity user, PasswordChange passwordChange)
		{
			if (PasswordHashing.ValidatePassword(passwordChange.OldPassword, user.Password))
			{
				user.Password = PasswordHashing.CalculateSha1(passwordChange.NewPassword1);
				Update(user);
				UnitOfWork.Save();
				return true;
			}

			return false;
		}

		public bool ResetPassword(ActionToken token, string password)
		{
			if (token.ValidUntil >= DateTime.Now)
			{
				var user = GetById(token.UserId);
				user.Password = PasswordHashing.CalculateSha1(password);
				Update(user);

				// reset token
				token.ValidUntil = DateTime.MinValue;
				UnitOfWork.ActionTokenRepository.Update(token);

				UnitOfWork.Save();
				return true;
			}

			return false;
		}

		#endregion

		#region Profile Utils

		public int GetProfileCompleteStatus(TEntity entity)
		{
			int completionFieldCount = 0;
			int completionValueCount = 0;
			var t = entity.GetType();
			foreach (var pi in t.GetProperties())
			{
				var dataTypeAttributes = pi.GetCustomAttribute<ProfileCompleteAttribute>();
				if (dataTypeAttributes != null)
				{
					completionFieldCount++;
					var val = pi.GetValue(entity);
					if (val != null && !string.IsNullOrEmpty(val.ToString()))
					{
						completionValueCount++;
					}
				}
			}

			var completeteStatus = (int)(((double)completionValueCount/completionFieldCount)*100);

			// check if profile is 100% complete and no bonus points where rewarded for that
			if (completeteStatus == 100)
			{
				var profileBonus =
					UnitOfWork.BonuspointRepository.AsQueryable().FirstOrDefault(b => b.Message == "Profildaten vollständig" && b.UserId == entity.Id);
				if (profileBonus == null)
				{
					// add bonuspoints for registration
					UnitOfWork.BonuspointRepository.Insert(entity.Id, BonusPointsFor.ProfileFull, "Profildaten vollständig");
				}
			}

			return completeteStatus;
		}

		#endregion

		#region Password Reset

		public void StartPasswordReset(string email)
		{
			var user = GetByEmail(email);
			if (user != null)
			{
				// create action token
				var resetToken = new ActionToken()
				{
					CreatedDate = DateTime.Now,
					Token = Guid.NewGuid(),
					Type = ActionTokenType.PasswordReset,
					UserId = user.Id,
					ValidUntil =
						DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["ActionToken.PasswordReset.Timeout"]))
				};
				UnitOfWork.ActionTokenRepository.Insert(resetToken);
				UnitOfWork.Save();

				// send e-mail with instructions
				var resetLink = string.Format(ConfigurationManager.AppSettings["BaseUrl"] + "/Login/PasswordReset?token={0}",
					resetToken.Token.ToString());
				var emailContent =
					string.Format(
						"Bitte klicke auf folgenden Link um dein Passwort zurückzusetzen:<br/><br/><a href='{0}' style='color:#0eb48d;'>{0}</a><br/><br/>Du bekommst dieses E-Mail weil du dein Passwort auf <a href='http://talentify.me' style='color:#0eb48d;'>talentify.me</a> zurückgesetzt hast. Solltest du das nicht gemacht haben, melde dich bitte unter <a href='mailto:hallo@talentify.at' style='color:#0eb48d;'>hallo@talentify.at</a> und gebe uns Bescheid.",
						resetLink);
				Email.Send(user.Email, WebConfigurationManager.AppSettings["Email.PasswordReset.Subject"], emailContent);
			}
		}

		#endregion

		#region Invites

		public FormFeedback SenInvite(ActionToken token, string toEmail)
		{
			try
			{
				var existingUser = GetByEmail(toEmail);
				if (existingUser != null)
				{
					return new FormFeedback()
					{
						IsError = true, 
						Headline = "E-Mail Adresse schon vorhanden", 
						Text = string.Format("Ein User mit der E-Mail Adresse {0} ist schon vorhanden.", toEmail)
					};
				}

				var user = GetById(token.UserId);
				var inviteUrl = string.Format("{0}/Register/Index?token={1}", ConfigurationManager.AppSettings["BaseUrl"], token.Token.ToString());
				var emailContent =
					string.Format(
						"Du wurdest von {0} {1} eingeladen dich bei talentify zu registrieren. Folge dem Link und registriere dich:<br/><br/><a href='{2}' style='color:#0eb48d;'>{2}</a>",
						user.Firstname, user.Surname, inviteUrl);
				var subject = string.Format(WebConfigurationManager.AppSettings["Email.Invite.Subject"], user.Firstname);
				Email.Send(toEmail, subject, emailContent, "Als Belohnung gibt's Bonuspunkte für dich und deinen Freund. Also gleich dem Link folgen und anmelden. Werde Teil der talentify-Community, entdecke deine Talente und hilf deinen MitschülerInnen.");
				return new FormFeedback() { IsError = false };
			}
			catch (Exception)
			{
				return new FormFeedback() { IsError = true };
			}
		}

		#endregion

		#region Talentecheck

		public TalentecheckSession GetTalentecheckSession(TEntity user)
		{
			return UnitOfWork.TalentecheckSessionRepository.AsQueryable().FirstOrDefault(s => s.UserId == user.Id);
		}

		#endregion

	}
}
