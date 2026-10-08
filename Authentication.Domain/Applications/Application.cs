namespace Authentication.Domain.Applications;

public sealed class Application
{
    private Application()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string ClientId { get; private set; } = null!;

    public Guid CreatedByUserId { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public static Application Create(
    string name,
    string clientId,
    Guid createdByUserId)
    {
        return new Application
        {
            Id = Guid.NewGuid(),
            Name = name,
            ClientId = clientId,
            CreatedByUserId = createdByUserId,
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