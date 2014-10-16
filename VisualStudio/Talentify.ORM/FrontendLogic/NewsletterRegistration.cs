using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailChimp.Net.Api;
using MailChimp.Net.Api.Domain;
using MailChimp.Net.Settings;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.FrontendLogic
{
	public static class NewsletterRegistration
	{
		public static void Subscribe(Student user)
		{
			var mailchimpApiService = new MailChimpApiService(MailChimpServiceConfiguration.Settings.ApiKey);

			var subscribeSources = new Grouping { Name = "Subscribe Source" };
			subscribeSources.Groups.Add("Platform");

			var gender = "Unbekannt";
			if (!string.IsNullOrEmpty(user.Gender) && user.Gender == "M")
			{
				gender = "Männlich";
			}
			else if (!string.IsNullOrEmpty(user.Gender) && user.Gender == "W")
			{
				gender = "Weiblich";
			}

			var school = (user.SchoolId.HasValue) ? user.SchoolId.ToString() : "Keine Schule";

			var fields = new Dictionary<string, string>
                    {
                        {"FNAME", !string.IsNullOrEmpty(user.Firstname) ? user.Firstname : string.Empty},
						{"LNAME", !string.IsNullOrEmpty(user.Surname) ? user.Surname : string.Empty},
						{"GESCHLECHT", gender},
						{"SCHULE", school},
						{"KLASSE", user.Class.ToString()},
						{"TELEFON", !string.IsNullOrEmpty(user.Phone) ? user.Phone : string.Empty}
                    };

			var response = mailchimpApiService.Subscribe(user.Email, new List<Grouping>() { subscribeSources }, fields, false);
		}

		public static void Unsubscribe(string email)
		{
			var mailchimpApiService = new MailChimpApiService(MailChimpServiceConfiguration.Settings.ApiKey);
			mailchimpApiService.Unsubscribe(email);
		}
	}
}
