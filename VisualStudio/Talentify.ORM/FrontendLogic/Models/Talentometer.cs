using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Models.Achievements;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class Talentometer
	{
		public TalentometerLevel CurrentLevel { get; set; }
		public TalentometerLevel NextLevel { get; set; }
		public int PercentFinished { get; set; }
		public int PercentOpen { get; set; }
	}
}
