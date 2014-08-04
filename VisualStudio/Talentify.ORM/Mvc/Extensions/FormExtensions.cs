using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Library;

namespace System.Web.Mvc.Html
{
	public static class FormExtensions
	{
		public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string prefix, bool useNearDates, DateTime? selectedDate)
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
				var selected = (selectedDate.HasValue && selectedDate.Value.Day == i) ? " selected" : string.Empty;
				output.Append(string.Format("<option value='{0}'{1}>{0}</option>", text, selected));
			}
			output.Append("</select>");

			// months
			output.Append(string.Format("<select name='{0}' id='{0}'>", monthId));
			output.Append("<option>MM</option>");
			for (int i = 1; i < 13; i++)
			{
				var text = (i < 10) ? "0" + i.ToString() : i.ToString();
				var selected = (selectedDate.HasValue && selectedDate.Value.Month == i) ? " selected" : string.Empty;
				output.Append(string.Format("<option value='{0}'{1}>{0}</option>", text, selected));
			}
			output.Append("</select>");

			// year
			output.Append(string.Format("<select name='{0}' id='{0}'>", yearId));
			output.Append("<option>YYYY</option>");
			var yearEnd = useNearDates ? DateTime.Now.Year - 1 : 1980;
			for (int i = DateTime.Now.Year + 1; i >= yearEnd; i--)
			{
				var selected = (selectedDate.HasValue && selectedDate.Value.Year == i) ? " selected" : string.Empty;
				output.Append(string.Format("<option value='{0}'{1}>{0}</option>", i, selected));
			}
			output.Append("</select>");

			return MvcHtmlString.Create(output.ToString());
		}

		public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string prefix, bool useNearDates)
		{
			return DatePicker(htmlHelper, prefix, useNearDates, DateTime.Now);
		}

		public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<IFormCheckable> sourceItems, IEnumerable<BaseEntity> selectedItems)
		{
			var output = new StringBuilder();
			output.Append("<div class='checkbox-list'>");
			output.Append(@"<ul>");

			foreach (var sourceItem in sourceItems)
			{
				var isChecked = string.Empty;
				if (selectedItems != null)
				{
					if (selectedItems.FirstOrDefault(i => i.Id == sourceItem.Id) != null)
						isChecked = "checked='checked'";
				}

				output.Append("<li>");
				output.Append(@"<label for='" + sourceItem.Id + "' class='checkbox'><input id='" + sourceItem.Id + "' name='" + name + "' type='checkbox' value='" + sourceItem.Id + "' " + isChecked + ">" + sourceItem.FormLabel + "</label>");
				output.Append("</li>");
			}

			output.Append("</ul>");
			output.Append("</div>");

			return MvcHtmlString.Create(output.ToString());
		}
	}
}
