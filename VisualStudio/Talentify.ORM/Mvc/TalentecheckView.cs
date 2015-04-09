using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Library;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.School;

namespace Talentify.ORM.Mvc
{
	public class TalentecheckView<T> : BaseView<T>
	{
		public TalentecheckBaseController TalentecheckBaseController
		{
			get { return BaseController as TalentecheckBaseController; }
		}

		public string ShareUrl
		{
			get { return this.BaseUrl + "/Talentecheck?s=" + TalentecheckBaseController.TalentecheckGuid.ToString(); }
		}
	}
}
