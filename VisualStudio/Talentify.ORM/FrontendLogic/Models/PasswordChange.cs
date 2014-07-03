using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class PasswordChange
	{
		public string OldPassword { get; set; }
		public string NewPassword1 { get; set; }
		public string NewPassword2 { get; set; }
	}
}
