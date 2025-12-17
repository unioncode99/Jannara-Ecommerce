using Jannara_Ecommerce.Business.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace Jannara_Ecommerce.Business.Services
{
    

    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            var email = _configuration["EMAIL_CONFIGURATION:EMAIL"];
            var password = _configuration["EMAIL_CONFIGURATION:PASSWORD"];
            var host = _configuration["EMAIL_CONFIGURATION:HOST"];
            var port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

            using var client = new SmtpClient();
            await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(email, password);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", email));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return true;
        }
    }
}
