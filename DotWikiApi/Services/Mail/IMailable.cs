using MimeKit;

namespace DotWikiApi.Services.Mail
{
    public interface IMailable
    {
        MimeMessage build();
    }
}