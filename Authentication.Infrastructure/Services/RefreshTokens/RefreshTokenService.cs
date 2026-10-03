using System.Security.Cryptography;
using System.Text;
using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Domain.Entities;
using Authentication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Authentication.Infrastructure.Services.RefreshTokens;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly AuthenticationDbContext _dbContext;
    private readonly ILogger<RefreshTokenService> _logger;

    public RefreshTokenService(
        AuthenticationDbContext dbContext,
        ILogger<RefreshTokenService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<string>> CreateAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);

        var token = Convert.ToBase64String(tokenBytes);

        var tokenHash = Convert.ToBase64String(
            SHA256.HashData(Encoding.UTF8.GetBytes(token)));

        var refreshToken = RefreshToken.Create(
            userId,
            tokenHash,
            DateTime.UtcNow.AddDays(7));

        await _dbContext.RefreshTokens.AddAsync(
            refreshToken,
            cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(token);
    }

    public async Task<Result<Guid>> ValidateAsync(
    string refreshToken,
    CancellationToken cancellationToken = default)
    {
        var tokenHash = HashToken(refreshToken);

        var token = await _dbContext.RefreshTokens
            .SingleOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);

        if (token is null)
        {
            _logger.LogWarning(
                "Refresh token validation rejected an unknown token.");
            return Result<Guid>.Failure(
                UserMessages.InvalidRefreshToken);
        }

        if (token.IsRevoked)
        {
            _logger.LogWarning(
                "Refresh token reuse detected; revoking all tokens for the account.");
            await RevokeAllAsync(
                token.UserId,
                cancellationToken);

            return Result<Guid>.Failure(
                UserMessages.RefreshTokenReuseDetected);
        }

        if (token.IsExpired)
        {
            return Result<Guid>.Failure(
                UserMessages.InvalidRefreshToken);
        }

        return Result<Guid>.Success(
            token.UserId);
    }

    public async Task RevokeAsync(
    string refreshToken,
    CancellationToken cancellationToken = default)
    {
        var tokenHash = HashToken(refreshToken);

        var storedToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(
                token => token.TokenHash == tokenHash,
                cancellationToken);

        if (storedToken is null)
            return;

        storedToken.Revoke();

        await _dbContext.SaveChangesAsync(
            cancellationToken);
    }

    public async Task<Result<bool>> RevokeAllAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userTokens = await _dbContext.RefreshTokens
            .Where(token => token.UserId == userId)
            .ToListAsync(cancellationToken);

        foreach (var token in userTokens)
        {
            token.Revoke();
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }

    private static string HashToken(string token)
    {
        return Convert.ToBase64String(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(token)));
    }
}