using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Models.Coaching;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class SearchSession
	{
		public SearchParams SearchParams { get; set; }
		public List<SearchResultItem> Items { get; set; }
		public SearchResultItem PrevItem { get; set; }
		public SearchResultItem NextItem { get; set; }
	}
}
