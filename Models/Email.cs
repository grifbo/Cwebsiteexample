using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace IHLA_Template.Models
{
	public class Email
	{
		public static void SendTourConfirmationEmail(string date, string time, string emailAddress)
		{
			// get account and pw
			string fromEmail = ConfigurationManager.AppSettings["fromEmail"];
			string emailSecret = ConfigurationManager.AppSettings["emailSecret"];

			// create email
			MailMessage mail = new MailMessage();
			SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
			mail.From = new MailAddress(fromEmail);
			mail.To.Add(emailAddress);
			mail.Subject = "Tour Confirmation";
			mail.Body = "Hello,\n\n" +
						"Thank you for choosing Inspiring Hearts.  We have your tour scheduled for " + date + " at " + time + ".  If anything changes, please call us at 513-222-2222.\n\n" +
						"Thank you,\n" +
						"Ashley and Toni";

			// establish connection
			SmtpServer.Port = 587;
			SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, emailSecret);
			SmtpServer.EnableSsl = true;

			// send email
			SmtpServer.Send(mail);

		}

		public static void SendApplicationConfirmationEmail(string emailAddress)
		{
			// get account and pw
			string fromEmail = ConfigurationManager.AppSettings["fromEmail"];
			string emailSecret = ConfigurationManager.AppSettings["emailSecret"];

			// create email
			MailMessage mail = new MailMessage();
			SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
			mail.From = new MailAddress(fromEmail);
			mail.To.Add(emailAddress);
			mail.Subject = "Tour Confirmation";
			mail.Body = "Hello,\n\n" +
						"Thank you for appliying with Inspiring Hearts. We will review your applicaiton and reach out to you regarding the next steps. If you need to contact us before then, please call us at 513-222-2222.\n\n" +
						"Thank you,\n" +
						"Ashley and Toni";

			// establish connection
			SmtpServer.Port = 587;
			SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, emailSecret);
			SmtpServer.EnableSsl = true;

			// send email
			SmtpServer.Send(mail);

		}

		public static void SendPaswordResetEmail(string emailAddress, string tempPass)
		{
			// get account and pw
			string fromEmail = ConfigurationManager.AppSettings["fromEmail"];
			string emailSecret = ConfigurationManager.AppSettings["emailSecret"];

			// create email
			MailMessage mail = new MailMessage();
			SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
			mail.From = new MailAddress(fromEmail);
			mail.To.Add(emailAddress);
			mail.Subject = "Password Reset";
			mail.Body = "Hello,\n\n" +
						"A request was made to reset your password. \n\n" +
						"Your temprary password is: " + tempPass + ".\n\n" +
						"Use this to reset your account by going to: [WEBSITE NAME]/profile/resetpassword \n\n\n" +
						"Thank you,\n" +
						"Ashley and Toni";

			// establish connection
			SmtpServer.Port = 587;
			SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, emailSecret);
			SmtpServer.EnableSsl = true;

			// send email
			SmtpServer.Send(mail);

		}
	}
}