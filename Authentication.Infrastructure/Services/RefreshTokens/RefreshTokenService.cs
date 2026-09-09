using System.Security.Cryptography;
using System.Text;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Domain.Entities;
using Authentication.Infrastructure.Persistence;

namespace Authentication.Infrastructure.Security.RefreshTokens;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly AuthenticationDbContext _dbContext;

    public RefreshTokenService(AuthenticationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> CreateAsync(
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

        return token;
    }

    public Task<bool> ValidateAsync(
        Guid userId,
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RevokeAsync(
        Guid userId,
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}