using Auth.Domain.Core.Logic.Models.Mail;

namespace Auth.Domain.Interface.Logic.External.Mail
{
    public interface IMailService
    {
        public Task SendAsync(EmailMessage message);
    }
}
