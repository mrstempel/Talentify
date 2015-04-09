using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.Talentecheck
{
	public class TalentecheckBonus : BaseEntity
	{
		public string Action { get; set; }
		public int Points { get; set; }
		public int TalentecheckSessionId { get; set; }
		private TalentecheckSession _talentecheckSession;
		public TalentecheckSession TalentecheckSession
		{
			get { return _talentecheckSession; }
			set
			{
				_talentecheckSession = value;
				if (value != null)
					TalentecheckSessionId = value.Id;
			}
		}
		public DateTime CreateDate { get; set; }
	}

	public class TalentecheckBonusMap : EntityTypeConfiguration<TalentecheckBonus>
	{
		public TalentecheckBonusMap()
		{
			// Table Name
			this.ToTable("TalentecheckBonus");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.CreateDate).HasColumnName("CreateDate").HasColumnType("datetime2");
		}
	}
}
