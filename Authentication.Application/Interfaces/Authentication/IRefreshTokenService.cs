namespace Authentication.Application.Interfaces.Authentication;

public interface IRefreshTokenService
{
    Task<string> CreateAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<bool> ValidateAsync(
        Guid userId,
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task RevokeAsync(
        Guid userId,
        string refreshToken,
        CancellationToken cancellationToken = default);
}