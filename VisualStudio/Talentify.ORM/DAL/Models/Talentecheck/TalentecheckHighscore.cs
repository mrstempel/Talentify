using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.Talentecheck
{
	public class TalentecheckHighscore : BaseEntity
	{
		public int TalentecheckSessionId { get; set; }
		public int Points { get; set; }
	}

	public class TalentecheckHighscoreMap : EntityTypeConfiguration<TalentecheckHighscore>
	{
		public TalentecheckHighscoreMap()
		{
			// Table Name
			this.ToTable("TalentecheckHighscores");
			// Primary Key
			this.HasKey(t => t.Id);
		}
	}
}
