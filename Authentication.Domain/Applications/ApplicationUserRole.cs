namespace Authentication.Domain.Applications;

public sealed class ApplicationUserRole
{
    private ApplicationUserRole()
    {
    }

    public Guid ApplicationId { get; private set; }

    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static ApplicationUserRole Create(
        Guid applicationId,
        Guid userId,
        Guid roleId)
    {
        return new ApplicationUserRole
        {
            ApplicationId = applicationId,
            UserId = userId,
            RoleId = roleId,
            CreatedAt = DateTime.UtcNow
        };
    }
}