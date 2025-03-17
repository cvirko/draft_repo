using Auth.Domain.Core.Logic.Models.Mail;

namespace Auth.Domain.Interface.Logic.Notification.Mail
{
    public interface IMailService
    {
        Task SendAsync(EmailMessage message);
    }
}
