using System.Security.Cryptography;
using System.Text;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Domain.Entities;
using Authentication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Services.RefreshTokens;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly AuthenticationDbContext _dbContext;

    public RefreshTokenService(AuthenticationDbContext dbContext)
    {
        _dbContext = dbContext;
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

    public async Task<bool> ValidateAsync(
           Guid userId,
           string refreshToken,
           CancellationToken cancellationToken = default)
    {
        var tokenHash = HashToken(refreshToken);

        var storedToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(
                token => token.UserId == userId &&
                          token.TokenHash == tokenHash,
                cancellationToken);

        return storedToken is not null && storedToken.IsActive;
    }

    public async Task RevokeAsync(
        Guid userId,
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var tokenHash = HashToken(refreshToken);

        var storedToken = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(
                token => token.UserId == userId &&
                          token.TokenHash == tokenHash,
                cancellationToken);

        if (storedToken is null)
            return;

        storedToken.Revoke();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static string HashToken(string token)
    {
        return Convert.ToBase64String(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(token)));
    }
}