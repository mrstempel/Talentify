using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talentify.ORM.Utils
{
	public class ProfileCompleteAttribute : DataTypeAttribute
	{
		public ProfileCompleteAttribute() : base(null) {}
		public ProfileCompleteAttribute(string name) : base(name) { }
	}
}
