using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Mvc.Html
{
	public static class FormExtensions
	{
		public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string prefix, bool useNearDates)
		{
			var output = new StringBuilder();
			var dayId = prefix + "_day";
			var monthId = prefix + "_month";
			var yearId = prefix + "_year";

			// days
			output.Append(string.Format("<select name='{0}' id='{0}'>", dayId));
			output.Append("<option>TT</option>");
			for (int i = 1; i < 32; i++)
			{
				var text = (i < 10) ? "0" + i.ToString() : i.ToString();
				output.Append(string.Format("<option value='{0}'>{0}</option>", text));
			}
			output.Append("</select>");

			// months
			output.Append(string.Format("<select name='{0}' id='{0}'>", monthId));
			output.Append("<option>MM</option>");
			for (int i = 1; i < 13; i++)
			{
				var text = (i < 10) ? "0" + i.ToString() : i.ToString();
				output.Append(string.Format("<option value='{0}'>{0}</option>", text));
			}
			output.Append("</select>");

			// year
			output.Append(string.Format("<select name='{0}' id='{0}'>", yearId));
			output.Append("<option>YYYY</option>");
			var yearStart = useNearDates ? DateTime.Now.Year - 1 : 1980;
			for (int i = yearStart; i < DateTime.Now.Year + 1; i++)
			{
				output.Append(string.Format("<option value='{0}'>{0}</option>", i));
			}
			output.Append("</select>");

			return MvcHtmlString.Create(output.ToString());
		}
	}
}
