using Authentication.Domain.Enums;

namespace Authentication.Domain.Applications;

public sealed class ApplicationSettings
{
    private ApplicationSettings() { }

    public Guid ApplicationId { get; private set; }

    // Email settings
    public EmailHandlingMode EmailHandlingMode { get; private set; }
    public bool IsEmailVerificationEnabled = true;

    // Future settings can be added here as the platform grows.

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static ApplicationSettings Create(
       Guid applicationId,
       bool isEmailVerificationEnabled = false,
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
            EmailHandlingMode = emailHandlingMode,
            IsEmailVerificationEnabled = isEmailVerificationEnabled,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(EmailHandlingMode emailHandlingMode,
        bool isEmailVerificationEnabled)
    {
        if (!Enum.IsDefined(emailHandlingMode))
        {
            throw new ArgumentOutOfRangeException(
                nameof(emailHandlingMode),
                "Email handling mode is invalid.");
        }

        EmailHandlingMode = emailHandlingMode;
        IsEmailVerificationEnabled = isEmailVerificationEnabled;
        UpdatedAt = DateTime.UtcNow;
    }
}
