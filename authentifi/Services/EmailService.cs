using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using authentifi.Models;

namespace authentifi.Services
{
	public class EmailService
	{
		private readonly EmailSettings _emailSettings;

		public EmailService(IOptions<EmailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value;
		}

		public async Task SendEmailAsync(string toEmail, string subject, string message)
		{
			var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
			{
				Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
				EnableSsl = true
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(_emailSettings.FromEmail),
				Subject = subject,
				Body = message,
				IsBodyHtml = true,
			};
			mailMessage.To.Add(toEmail);

			await client.SendMailAsync(mailMessage);
		}
	}
}
