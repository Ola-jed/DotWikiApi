using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;

namespace DotWikiApi.Services.Mail
{
    public class MailService: IMailService
    {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(IMailable mailable)
        {
            var email = mailable.Build();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_settings.Host,_settings.Port,SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.MailUser,_settings.MailPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}