using Microsoft.Extensions.Options;
using Backend.Application.Common.Settings;

namespace Backend.Application.Common.Services;

public interface IEmailService
{
}

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }
}
