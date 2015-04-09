using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.Talentecheck
{
	public enum TalentecheckTyp
	{
		Sport,
		Creative,
		Rockstar,
		Nerd,
		Weltverbesserer,
		Kommunikator,
		Fashionista,
		Brain
	}

	public class TalentecheckSession : BaseEntity
	{
		public Guid SessionId { get; set; }
		public Guid InviteSessionId { get; set; }
		public int Frage1 { get; set; }
		public int Frage2 { get; set; }
		public int Frage3 { get; set; }
		public int Frage4 { get; set; }
		public int Frage5 { get; set; }
		public int Frage6 { get; set; }
		public int Frage7 { get; set; }
		public int Frage8 { get; set; }
		public int Frage9 { get; set; }
		public int Frage10 { get; set; }
		public bool IsFinished { get; set; }
		public TalentecheckTyp TypMax { get; set; }
		public int MaxPercent { get; set; }
		public TalentecheckTyp TypMin { get; set; }
		public int MinPercent { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime FinishTime { get; set; }
		public int UserId { get; set; }
	}

	public class TalentecheckSessionMap : EntityTypeConfiguration<TalentecheckSession>
	{
		public TalentecheckSessionMap()
		{
			// Table Name
			this.ToTable("TalentecheckSession");
			// Primary Key
			this.HasKey(t => t.Id);

			this.Property(t => t.StartTime).HasColumnName("StartTime").HasColumnType("datetime2");
			this.Property(t => t.FinishTime).HasColumnName("FinishTime").HasColumnType("datetime2");
		}
	}
}
