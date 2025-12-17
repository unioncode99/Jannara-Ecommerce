namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IEmailSenderService
    {
        public Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
