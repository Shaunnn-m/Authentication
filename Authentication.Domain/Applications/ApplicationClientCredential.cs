namespace Authentication.Domain.Applications;

public sealed class ApplicationClientCredential
{
    private const int SecretHashLength = 32;

    private ApplicationClientCredential()
    {
    }

    public Guid Id { get; private set; }

    public Guid ApplicationId { get; private set; }

    public string Name { get; private set; } = null!;

    public byte[] SecretHash { get; private set; } = [];

    public DateTime CreatedAt { get; private set; }

    public DateTime? ExpiresAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public DateTime? LastUsedAt { get; private set; }

    public bool IsActive =>
        !RevokedAt.HasValue &&
        (!ExpiresAt.HasValue || ExpiresAt.Value > DateTime.UtcNow);

    public static ApplicationClientCredential Create(
        Guid applicationId,
        string name,
        byte[] secretHash,
        DateTime? expiresAt = null)
    {
        if (applicationId == Guid.Empty)
        {
            throw new ArgumentException(
                "Application ID is required.",
                nameof(applicationId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Credential name is required.",
                nameof(name));
        }

        if (name.Length > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(name),
                "Credential name cannot exceed 100 characters.");
        }

        if (secretHash is null || secretHash.Length != SecretHashLength)
        {
            throw new ArgumentException(
                $"Secret hash must be exactly {SecretHashLength} bytes.",
                nameof(secretHash));
        }

        if (expiresAt.HasValue && expiresAt.Value <= DateTime.UtcNow)
        {
            throw new ArgumentOutOfRangeException(
                nameof(expiresAt),
                "Credential expiration must be in the future.");
        }

        return new ApplicationClientCredential
        {
            Id = Guid.NewGuid(),
            ApplicationId = applicationId,
            Name = name.Trim(),
            SecretHash = secretHash.ToArray(),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt
        };
    }

    public void Revoke()
    {
        RevokedAt ??= DateTime.UtcNow;
    }

    public void RecordUsage()
    {
        if (!IsActive)
        {
            throw new InvalidOperationException(
                "Inactive credentials cannot be used.");
        }

        LastUsedAt = DateTime.UtcNow;
    }
}
