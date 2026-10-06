using Authentication.Application.Abstractions.Results.RefreshToken;
using Authentication.Application.Common.Results;

namespace Authentication.Application.Interfaces.Authentication;

public interface IRefreshTokenService
{
    Task<Result<string>> CreateAsync(
        Guid userId, 
        Guid applicationId,
        CancellationToken cancellationToken = default);

    Task<Result<RefreshTokenValidationResult>> ValidateAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task RevokeAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task<Result<bool>> RevokeAllAsync(
    Guid userId,
    CancellationToken cancellationToken = default);
}