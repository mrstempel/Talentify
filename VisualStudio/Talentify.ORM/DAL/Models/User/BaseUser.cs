using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Models.User
{
	public class BaseUser : BaseEntity
	{
		[ProfileComplete("E-Mail")]
		public string Email { get; set; }
		public string Password { get; set; }
		[ProfileComplete("Vorname")]
		public string Firstname { get; set; }
		[ProfileComplete("Nachname")]
		public string Surname { get; set; }
		[ProfileComplete("BirthDate")]
		public DateTime? BirthDate { get; set; }
		[ProfileComplete("Adressse")]
		public string Address { get; set; }
		[ProfileComplete("PLZ")]
		public string ZipCode { get; set; }
		[ProfileComplete("Ort")]
		public string City { get; set; }
		[ProfileComplete("Land")]
		public string Country { get; set; }
		[ProfileComplete("Telefon")]
		public string Phone { get; set; }
		public Guid? RegisterCode { get; set; }
		public DateTime JoinedDate { get; set; }
		public DateTime? UpdateDate { get; set; }
		public virtual ICollection<Subscription> Subscriptions { get; set; }
		[ProfileComplete("Gender")]
		public string Gender { get; set; }

		public int? SettingsId { get; set; }
		private UserSettings _settings;
		public virtual UserSettings Settings
		{
			get { return _settings; }
			set
			{
				_settings = value;
				if (value != null)
					SettingsId = value.Id;
			}
		}

		[ProfileComplete("PictureGuid")]
		public Guid? PictureGuid { get; set; }
		public bool IsPictureLandscape { get; set; }

		public bool IsDeleted { get; set; }

		public ICollection<BonusPoint> BonusPoints { get; set; }
		public virtual ICollection<Badge> Badges { get; set; } 

		#region Frontend Properties

		// frontend properties
		public string FormattedAddress
		{
			get
			{
				var address = this.Address;
				
				if (!string.IsNullOrEmpty(address))
					address += ", ";
				if (!string.IsNullOrEmpty(ZipCode))
					address += ZipCode + "-";
				if (!string.IsNullOrEmpty(City))
					address += City;

				return address;
			}
		}

		public bool HasProfilePicture { get { return PictureGuid.HasValue;  } }
		public string ProfileImageSmall
		{
			get
			{
				return (HasProfilePicture) ?
					string.Format("{0}{1}_small.png", ConfigurationManager.AppSettings["Upload.Profile"], PictureGuid.ToString()) :
					"/Images/default-profile-small.png";
			}
		}

		public string ProfileImageMedium
		{
			get
			{
				return (HasProfilePicture) ?
					string.Format("{0}{1}_medium.png", ConfigurationManager.AppSettings["Upload.Profile"], PictureGuid.ToString()) :
					"/Images/default-profile-medium.png";
			}
		}

		public string ProfileImageLarge
		{
			get
			{
				return (HasProfilePicture) ?
					string.Format("{0}{1}_large.png", ConfigurationManager.AppSettings["Upload.Profile"], PictureGuid.ToString()) :
					"/Images/default-profile-large.png";
			}
		}

		#endregion
	}

	public class BaseUserMap : EntityTypeConfiguration<BaseUser>
	{
		public BaseUserMap()
		{
			// Table Name
			this.ToTable("BaseUser");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.BirthDate).HasColumnName("BirthDate").HasColumnType("datetime2");
			this.Property(t => t.JoinedDate).HasColumnName("JoinedDate").HasColumnType("datetime2");
			this.Property(t => t.UpdateDate).HasColumnName("UpdateDate").HasColumnType("datetime2");
		}
	}
}
