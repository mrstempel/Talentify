﻿using System;
using System.Collections.Generic;
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

		public virtual FormFeedback Register(TEntity user)
		{
			try
			{
				// set dates
				user.JoinedDate = DateTime.Now;
				// create register code
				user.RegisterCode = Guid.NewGuid();
				// set default settings
				if (!user.SettingsId.HasValue && user.Settings == null)
				{
					AddDefaultSettings(user);
				}
				// insert user
				Insert(user);
				// create default subscription
				var subscription = new Subscription()
				{
					Membership = UnitOfWork.MembershipRepository.AsQueryable().FirstOrDefault(m => m.Type == MembershipType.Free),
					User = user,
					PurchaseDate = DateTime.Now
				};
				// insert default subscription
				UnitOfWork.SubscriptionRepository.Insert(subscription);
				// commit to database
				UnitOfWork.Save();

				return new FormFeedback() { IsError = false };
			}
			catch (Exception ex)
			{
				
				return new FormFeedback() { IsError = true, Text = ex.Message };
			}
		}

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
			return completeteStatus;
		}

		#endregion
	}
}
