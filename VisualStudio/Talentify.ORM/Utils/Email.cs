using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Talentify.ORM.Utils
{
	public static class Email
	{
		public static void Send(MailMessage msg)
		{
			if (Convert.ToBoolean(WebConfigurationManager.AppSettings["Email.Enabled"]))
			{
				var smtpClient = new SmtpClient();
				smtpClient.Send(msg);
			}
		}
	}
}
