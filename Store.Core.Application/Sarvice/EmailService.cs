using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Threading.Tasks;
using Store.Core.Application.Sarvice.ISarvice;
using Microsoft.Extensions.Logging;

namespace Store.Core.Application.Sarvice
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings), "Email settings are missing in the configuration.");
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
             if (string.IsNullOrEmpty(toEmail) || !toEmail.Contains("@"))
            {
                throw new ArgumentException("Invalid recipient email address.", nameof(toEmail));
            }

             if (string.IsNullOrEmpty(_emailSettings.SenderEmail) || !_emailSettings.SenderEmail.Contains("@"))
            {
                throw new ArgumentException("Invalid sender email address in configuration.", nameof(_emailSettings.SenderEmail));
            }

            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentNullException(nameof(subject), "Email subject cannot be null or empty.");
            }

            if (string.IsNullOrEmpty(body))
            {
                throw new ArgumentNullException(nameof(body), "Email body cannot be null or empty.");
            }

            var emailMessage = new MimeMessage();

             emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await client.SendAsync(emailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SMTP ERROR: {ex.Message}");
                Console.WriteLine($"STACK TRACE: {ex.StackTrace}");
                throw new Exception($"Error sending email: {ex.Message}");
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

    }
    public class EmailSettings
    {
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
