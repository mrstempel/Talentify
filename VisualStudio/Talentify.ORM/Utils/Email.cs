using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;

namespace Talentify.ORM.Utils
{
	public static class Email
	{
		public static void Send(MailMessage msg)
		{
			if (Convert.ToBoolean(WebConfigurationManager.AppSettings["Email.Enabled"]))
			{
				msg.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["Email.ReplyTo"]));
				var smtpClient = new SmtpClient();
				smtpClient.Send(msg);
			}
		}

		public static void Send(string to, string subject, string content, string ciao = null)
		{
			var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/Email.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);
			templateContent = templateContent.Replace("{Content}", content);
			templateContent = templateContent.Replace("{Ciao}", ciao ?? "Viel Spaß beim Talente entdecken!");

			var msg = new MailMessage(ConfigurationManager.AppSettings["Email.From"], to)
			{
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			Send(msg);
		}

		public static void SendToTeacher(string to, string subject, string content)
		{
			var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/EmailTeacher.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);
			templateContent = templateContent.Replace("{Content}", content);

			var msg = new MailMessage(ConfigurationManager.AppSettings["Email.From"], to)
			{
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			Send(msg);
		}

		public static void SendDelete(string to, string subject)
		{
			var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/EmailDelete.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);

			var msg = new MailMessage(ConfigurationManager.AppSettings["Email.From"], to)
			{
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			Send(msg);
		}

		public static void SendBlocked(string to, string subject, string reason)
		{
			var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/EmailBlocked.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);
			templateContent = templateContent.Replace("{BlockedReason}", reason);

			var msg = new MailMessage(ConfigurationManager.AppSettings["Email.From"], to)
			{
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			Send(msg);
		}

		public static void SendNotification(string to, string subject, string icon, string link, string profileImage, string text)
		{
			var reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Templates/EmailNotification.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);
			if (!string.IsNullOrEmpty(icon))
			{
				templateContent = templateContent.Replace("{n-icon}", string.Format("<td style='text-align: right;'><img src='{0}/Images/{1}' style='float: right'/></td>", ConfigurationManager.AppSettings["BaseUrl"], icon));
			}
			else
			{
				templateContent = templateContent.Replace("{n-icon}", string.Empty);
			}
			templateContent = templateContent.Replace("{n-link}", link);
			templateContent = templateContent.Replace("{n-profile-image}", profileImage);
			templateContent = templateContent.Replace("{n-text}", text);

			var msg = new MailMessage(ConfigurationManager.AppSettings["Email.From"], to)
			{
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			Send(msg);
		}

		public static void SenEventNotification(string to, string subject, string title, string tag, string datum,
			string uhrzeitVon, string uhrzeitBis, string address, string link)
		{
			var reader = new StreamReader(HostingEnvironment.MapPath("~/Templates/EmailWorkshopReminder.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);
			templateContent = templateContent.Replace("{Title}", title);
			templateContent = templateContent.Replace("{Tag}", tag);
			templateContent = templateContent.Replace("{Datum}", datum);
			templateContent = templateContent.Replace("{UhrzeitVon}", uhrzeitVon);
			templateContent = templateContent.Replace("{UhrzeitBis}", uhrzeitBis);
			templateContent = templateContent.Replace("{Adresse}", address);
			templateContent = templateContent.Replace("{Link}", link);

			var msg = new MailMessage(ConfigurationManager.AppSettings["Email.From"], to)
			{
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			Send(msg);
		}

		public static void SenEventConfirmedEmail(string to, string subject, string title, int id)
		{
			var reader = new StreamReader(HostingEnvironment.MapPath("~/Templates/EmailEventConfirmed.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);
			templateContent = templateContent.Replace("{Title}", title);
			templateContent = templateContent.Replace("{Id}", id.ToString());

			var msg = new MailMessage(ConfigurationManager.AppSettings["Email.From"], to)
			{
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			Send(msg);
		}

		public static void SenEventNotConfirmed1Email(string to, string subject, string title, string tag, string datum,
			string uhrzeitVon, string uhrzeitBis, int registerId)
		{
			var reader = new StreamReader(HostingEnvironment.MapPath("~/Templates/EmailEventNotConfirmed1.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);
			templateContent = templateContent.Replace("{Title}", title);
			templateContent = templateContent.Replace("{Tag}", tag);
			templateContent = templateContent.Replace("{Datum}", datum);
			templateContent = templateContent.Replace("{UhrzeitVon}", uhrzeitVon);
			templateContent = templateContent.Replace("{UhrzeitBis}", uhrzeitBis);
			templateContent = templateContent.Replace("{RegisterId}", registerId.ToString());

			var msg = new MailMessage(ConfigurationManager.AppSettings["Email.From"], to)
			{
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			Send(msg);
		}

		public static void SenEventNotConfirmed2Email(string to, string subject, string title, string tag, string datum,
			string uhrzeitVon, string uhrzeitBis, int registerId)
		{
			var reader = new StreamReader(HostingEnvironment.MapPath("~/Templates/EmailEventNotConfirmed2.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);
			templateContent = templateContent.Replace("{Title}", title);
			templateContent = templateContent.Replace("{Tag}", tag);
			templateContent = templateContent.Replace("{Datum}", datum);
			templateContent = templateContent.Replace("{UhrzeitVon}", uhrzeitVon);
			templateContent = templateContent.Replace("{UhrzeitBis}", uhrzeitBis);
			templateContent = templateContent.Replace("{RegisterId}", registerId.ToString());

			var msg = new MailMessage(ConfigurationManager.AppSettings["Email.From"], to)
			{
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			Send(msg);
		}

		public static void SendAccountConfirmed(string to, string subject)
		{
			var reader = new StreamReader(HostingEnvironment.MapPath("~/Templates/EmailAccountConfirmed.html"));
			string templateContent = reader.ReadToEnd();
			reader.Close();

			templateContent = templateContent.Replace("{BaseUrl}", ConfigurationManager.AppSettings["BaseUrl"]);

			var msg = new MailMessage(ConfigurationManager.AppSettings["Email.From"], to)
			{
				Subject = subject,
				Body = templateContent,
				IsBodyHtml = true
			};
			Send(msg);
		}
	}
}
