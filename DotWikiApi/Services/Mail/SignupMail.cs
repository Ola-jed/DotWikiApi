using DotWikiApi.Models;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DotWikiApi.Services.Mail;

public class SignupMail : IMailable
{
    private readonly MailSettings _options;
    private readonly ApplicationUser _user;

    public SignupMail(IOptions<MailSettings> mailSettings, ApplicationUser user)
    {
        _options = mailSettings.Value;
        _user = user;
    }

    public MimeMessage Build()
    {
        var email = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_options.MailUser),
            Subject = "Registration to DotWiki",
            To = { MailboxAddress.Parse(_user.Email) }
        };
        var builder = new BodyBuilder
        {
            HtmlBody = $"Hello {_user.UserName}. We are happy to welcome you among us at DotWiki.",
            TextBody = $"Hello {_user.UserName}. We are happy to welcome you among us at DotWiki."
        };
        email.Body = builder.ToMessageBody();
        return email;
    }
}