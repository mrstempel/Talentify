using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Talentify.ORM.DAL.Models.Talentecheck;

namespace Talentify.Web.Models.Talentecheck
{
	public class ResultPageData
	{
		public string PlusType { get; set; }
		public string PlusTypeReadable { get; set; }
		public int PlusPercent { get; set; }
		public string MinusType { get; set; }
		public string MinusTypeReadable { get; set; }
		public int MinusPercent { get; set; }
		public int Sex { get; set; }

		public Dictionary<TalentecheckTyp, int> TypeOverview { get; set; }
		public IEnumerable<TalentecheckHighscoreItem> HighscoreItems { get; set; }
	}
}