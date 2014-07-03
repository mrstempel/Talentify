using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Content
{
	public class BasePage : BaseEntity
	{
		public string Title { get; set; }
		public string Headline { get; set; }
		public string PreviewImage { get; set; }
		public string Image { get; set; }
		public string Content { get; set; }
		public bool IsOnline { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdateDate { get; set; }

		public int CreatedById { get; set; }
		private BaseUser _createdBy;
		public BaseUser CreatedBy
		{
			get { return _createdBy; }
			set
			{
				_createdBy = value;
				if (value != null)
					CreatedById = value.Id;
			}
		}

		public int? UpdateById { get; set; }
		private BaseUser _updateBy;
		public BaseUser UpdateBy
		{
			get { return _updateBy; }
			set
			{
				_updateBy = value;
				if (value != null)
					UpdateById = value.Id;
			}
		}
	}

	public class BasePageMap : EntityTypeConfiguration<BasePage>
	{
		public BasePageMap()
		{
			// Table Name
			this.ToTable("BasePage");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreatedDate).HasColumnName("JoinedDate").HasColumnType("datetime2");
			this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").HasColumnType("datetime2");
		}
	}
}
