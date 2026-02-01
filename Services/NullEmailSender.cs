using Microsoft.AspNetCore.Identity.UI.Services;

namespace IT_Helpdesk_System.Services
{
    public class NullEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // No-op: we are not sending emails yet
            return Task.CompletedTask;
        }
    }
}
