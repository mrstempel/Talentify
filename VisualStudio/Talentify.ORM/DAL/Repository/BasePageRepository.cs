using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Content;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class BasePageRepository<TEntity> : TalentifyRepository<TEntity> where TEntity : BasePage 
	{
		public BasePageRepository(TalentifyContext context)
            : base(context)
        {
        }

		public void Insert(TEntity entity, HttpPostedFileBase uploadPreview, HttpPostedFileBase uploadImage, string uploadPath)
		{
			// set created date
			entity.CreatedDate = DateTime.Now;

			// save files
			if (uploadPreview.ContentLength > 0)
			{
				entity.PreviewImage = Guid.NewGuid() + Path.GetExtension(uploadPreview.FileName);
				uploadPreview.SaveAs(Path.Combine(uploadPath, entity.PreviewImage));
			}

			if (uploadImage.ContentLength > 0)
			{
				entity.Image = Guid.NewGuid() + Path.GetExtension(uploadImage.FileName);
				uploadImage.SaveAs(Path.Combine(uploadPath, entity.Image));
			}

			base.Insert(entity);
		}

		public void Update(TEntity entity, HttpPostedFileBase uploadPreview, HttpPostedFileBase uploadImage, string uploadPath)
		{
			// set created date
			entity.UpdateDate = DateTime.Now;

			// save files
			if (uploadPreview != null && uploadPreview.ContentLength > 0)
			{
				entity.PreviewImage = Guid.NewGuid() + Path.GetExtension(uploadPreview.FileName);
				uploadPreview.SaveAs(Path.Combine(uploadPath, entity.PreviewImage));
			}

			if (uploadImage != null && uploadImage.ContentLength > 0)
			{
				entity.Image = Guid.NewGuid() + Path.GetExtension(uploadImage.FileName);
				uploadImage.SaveAs(Path.Combine(uploadPath, entity.Image));
			}

			base.Update(entity);
		}
	}
}
