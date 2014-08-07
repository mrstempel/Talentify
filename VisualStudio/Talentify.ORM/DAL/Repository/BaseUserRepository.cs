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

		#region Register

		public void AddDefaultSettings(TEntity user)
		{
			user.Settings = new UserSettings() { HasNewsletter = true, HasNotifications = true };
		}

		public virtual BaseUser RegisterConfirm(Guid registerCode)
		{
			var user = UnitOfWork.BaseUserRepository.AsQueryable().FirstOrDefault(u => u.RegisterCode == registerCode);
			
			if (user != null)
			{
				user.RegisterCode = null;
				UnitOfWork.Save();

				// add greenhorn badge
				UnitOfWork.BadgeRepository.AddBadgeToUser(user, "Greenhorn");

				// add bonuspoints for registration
				UnitOfWork.BonuspointRepository.Insert(user.Id, BonusPointsFor.Register, "Erfolgreiche Registrierung");
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
				var mailMsg = new MailMessage(WebConfigurationManager.AppSettings["Email.From"], user.Email);
				mailMsg.Subject = WebConfigurationManager.AppSettings["Email.PasswordReset.Subject"];
				var resetLink = string.Format(ConfigurationManager.AppSettings["BaseUrl"] + "/Login/PasswordReset?token={0}",
					resetToken.Token.ToString());
				mailMsg.Body = string.Format("Link: {0}", resetLink);
				Email.Send(mailMsg);
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

				var mailMsg = new MailMessage(WebConfigurationManager.AppSettings["Email.From"], toEmail);
				mailMsg.Subject = WebConfigurationManager.AppSettings["Email.Invite.Subject"];
				var inviteUrl = string.Format("{0}/Register/Index?token={1}", ConfigurationManager.AppSettings["BaseUrl"], token.Token.ToString());
				mailMsg.Body = inviteUrl;
				Email.Send(mailMsg);
				return new FormFeedback() { IsError = false };
			}
			catch (Exception)
			{
				return new FormFeedback() { IsError = true };
			}
		}

		#endregion
	}
}
