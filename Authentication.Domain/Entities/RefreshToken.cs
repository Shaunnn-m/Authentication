using Authentication.Domain.Common;

namespace Authentication.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public Guid UserId { get; private set; }

    public string TokenHash { get; private set; } = string.Empty;

    public DateTime ExpiresAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public bool IsRevoked => RevokedAt.HasValue;

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsActive => !IsRevoked && !IsExpired;

    private RefreshToken()
    {
        // Required by EF Core.
    }

    private RefreshToken(
        Guid userId,
        string tokenHash,
        DateTime expiresAt)
    {
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
    }

    public static RefreshToken Create(
        Guid userId,
        string tokenHash,
        DateTime expiresAt)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(
                "User ID is required.",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new ArgumentException(
                "Token hash is required.",
                nameof(tokenHash));

        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException(
                "Refresh token must expire in the future.",
                nameof(expiresAt));

        return new RefreshToken(
            userId,
            tokenHash,
            expiresAt);
    }

    public void Revoke()
    {
        if (IsRevoked)
            return;

        RevokedAt = DateTime.UtcNow;
    }
}