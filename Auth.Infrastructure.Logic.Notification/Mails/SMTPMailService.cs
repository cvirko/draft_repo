using Auth.Domain.Core.Common.Consts;
using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Common.Tools.Configurations;
using Auth.Domain.Core.Logic.Models.Mail;
using Auth.Domain.Interface.Logic.Notification.Mail;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Text;

namespace Auth.Infrastructure.Logic.Notification.Mails
{
    internal class SMTPMailService : IMailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly MailOptions _config;
        public SMTPMailService(IOptions<MailOptions> option)
        {
            _config = option.Value;
            _smtpClient = new SmtpClient
            {
                Host = _config.Host,
                Port = _config.Port,
                EnableSsl = _config.EnableSsl,
                UseDefaultCredentials = _config.UseDefaultCredentials,
                Credentials = new System.Net.NetworkCredential(_config.UserName, _config.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };

        }
        public async Task SendAsync(EmailMessage message)
        {
            MailMessage msg = new MailMessage();
            var serviceMail = new MailAddress(_config.Email, _config.TeamName);

            msg.From = serviceMail;
            msg.To.Add(new MailAddress(message.EmailTo, message.UserName));
            msg.Subject = $"{_config.TeamName} - {message.MessageType}.";
            msg.IsBodyHtml = true;

            LinkedResource linkedResource = new LinkedResource(_config.BackgroundImgURL);
            linkedResource.ContentId = Guid.NewGuid().ToString();
            linkedResource.ContentType = new(MIMEType.Jpep);


            var html = new StringBuilder(File.ReadAllText(_config.MessageHtmlURL));
            html = html.Replace("//CodeReplace//", message.Token);
            html = html.Replace("//Year//", DateTimeExtension.Get().Year.ToString());
            html = html.Replace("//TeamName//", _config.TeamName);
            html = html.Replace("//Subject//", message.MessageType.ToString());
            html = html.Replace("//BackGroundImg//", $"cid:{linkedResource.ContentId}");
            AlternateView av = AlternateView.CreateAlternateViewFromString(
                html.ToString(), null, MIMEType.Html);
            av.LinkedResources.Add(linkedResource);
            msg.AlternateViews.Add(av);

            await SendMailAsync(msg);
        }
        private async Task SendMailAsync(MailMessage msg, int attempts = 3, int delay = 10)
        {
            try
            {
                await _smtpClient.SendMailAsync(msg);
                attempts--;
            }
            catch (Exception ex)
            {
                if (attempts == 0)
                    throw new Exception($"Sending mail failed! {ex.Message}");
                await Task.Delay(TimeSpan.FromSeconds(delay));
                await SendMailAsync(msg, attempts);
            }
        }
    }
}
