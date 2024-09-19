using System.Net;
using System.Net.Mail;

namespace PresentationLayer.Utilities
{
	public class Email
	{
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Recipient { get; set; }
    }
	public static class MailSettings
	{
		public static void SendEmail(Email email)
		{
			// Create SMTP Client
			var client = new SmtpClient("smtp.gmail.com",587);
			client.EnableSsl = true;
			// Create Network Credentials
			client.Credentials = new NetworkCredential("youssef5fox5@gmail.com"," ");
			
			client.Send("youssef5fox5@gmail.com" , email.Recipient,email.Subject, email.Body);
		}
	}
}