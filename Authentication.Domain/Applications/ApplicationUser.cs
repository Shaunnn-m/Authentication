namespace Authentication.Domain.Applications;

public sealed class ApplicationUserAccess
{
    private ApplicationUserAccess()
    {
    }

    public Guid ApplicationId { get; private set; }

    public Guid UserId { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public static ApplicationUserAccess Create(
        Guid applicationId,
        Guid userId)
    {
        return new ApplicationUserAccess
        {
            ApplicationId = applicationId,
            UserId = userId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}