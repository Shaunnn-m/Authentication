using Authentication.Domain.Enums;

namespace Authentication.Domain.Applications;

public sealed class ApplicationSettings
{
    private ApplicationSettings()
    {
    }

    public Guid ApplicationId { get; private set; }

    public bool IsEmailVerificationEnabled { get; private set; }

    public EmailHandlingMode EmailHandlingMode { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static ApplicationSettings Create(
        Guid applicationId,
        bool isEmailVerificationEnabled = true,
        EmailHandlingMode emailHandlingMode = EmailHandlingMode.PlatformManaged)
    {
        if (applicationId == Guid.Empty)
        {
            throw new ArgumentException(
                "Application ID is required.",
                nameof(applicationId));
        }

        if (!Enum.IsDefined(emailHandlingMode))
        {
            throw new ArgumentOutOfRangeException(
                nameof(emailHandlingMode),
                "Email handling mode is invalid.");
        }

        return new ApplicationSettings
        {
            ApplicationId = applicationId,
            IsEmailVerificationEnabled = isEmailVerificationEnabled,
            EmailHandlingMode = emailHandlingMode,
            CreatedAt = DateTime.UtcNow
        };
    }
}
