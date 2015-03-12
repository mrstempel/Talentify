using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;

namespace Talentify.ORM.DAL.Models.Talentecheck
{
	public enum TalentecheckTyp
	{
		Sport,
		Kunst,
		Musik,
		It,
		Peace,
		Kommunikation,
		Fashion,
		Brain
	}

	public class TalentecheckSession : BaseEntity
	{
		public Guid SessionId { get; set; }
		public int Frage1 { get; set; }
		public int Frage2 { get; set; }
		public int Frage3 { get; set; }
		public int Frage4 { get; set; }
		public int Frage5 { get; set; }
		public int Frage6 { get; set; }
		public int Frage7 { get; set; }
		public int Frage8 { get; set; }
		public int Frage9 { get; set; }
		public string Sex { get; set; }
		public bool IsFinished { get; set; }
		public TalentecheckTyp TypMax { get; set; }
		public TalentecheckTyp TypMin { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime FinishTime { get; set; }
		public int UserId { get; set; }
	}
}
