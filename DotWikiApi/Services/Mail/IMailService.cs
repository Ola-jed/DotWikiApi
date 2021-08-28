using System.Threading.Tasks;

namespace DotWikiApi.Services.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(IMailable mailable);
    }
}