namespace Authentication.Domain.Applications;

public sealed class ApplicationSettings
{
    private ApplicationSettings() { }

    public Guid ApplicationId { get; private set; }

    // Email settings
    public bool EmailVerificationEnabled { get; private set; }
    public bool PasswordResetEnabled { get; private set; }

    // Account settings
    public bool AllowRegistration { get; private set; }

    // Future settings can be added here as the platform grows.

    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public static ApplicationSettings CreateDefault(Guid applicationId)
    {
        return new ApplicationSettings
        {
            ApplicationId = applicationId,
            EmailVerificationEnabled = true,
            PasswordResetEnabled = true,
            AllowRegistration = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        bool emailVerificationEnabled,
        bool passwordResetEnabled,
        bool allowRegistration)
    {
        EmailVerificationEnabled = emailVerificationEnabled;
        PasswordResetEnabled = passwordResetEnabled;
        AllowRegistration = allowRegistration;
        UpdatedAt = DateTime.UtcNow;
    }
}