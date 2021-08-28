using System.Threading.Tasks;

namespace DotWikiApi.Services.Mail
{
    public class MailService: IMailService
    {
        public Task SendEmailAsync(IMailable mailable)
        {
            throw new System.NotImplementedException();
        }
    }
}