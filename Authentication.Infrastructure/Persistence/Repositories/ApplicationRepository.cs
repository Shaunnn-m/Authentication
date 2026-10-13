using Authentication.Application.Interfaces.Applications;
using ApplicationEntity = Authentication.Domain.Applications.Application;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence.Repositories;

public sealed class ApplicationRepository : IApplicationRepository
{
    private readonly AuthenticationDbContext _context;

    public ApplicationRepository(AuthenticationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        ApplicationEntity application,
        CancellationToken cancellationToken = default)
    {
        await _context.Applications.AddAsync(
            application,
            cancellationToken);
    }

    public async Task<ApplicationEntity?> GetByClientIdAsync(
        string clientId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Applications
            .FirstOrDefaultAsync(
                application => application.ClientId == clientId,
                cancellationToken);
    }

    public async Task<ApplicationEntity?> GetByAppplicationId(
        Guid ApplicationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Applications
            .FirstOrDefaultAsync(
                    application => application.Id == ApplicationId,
                    cancellationToken);
    }

    public async Task DeleteAsync(
        ApplicationEntity application,
        CancellationToken cancellationToken = default)
    {
        var applicationUserRoles = await _context.ApplicationUserRoles
            .Where(userRole => userRole.ApplicationId == application.Id)
            .ToListAsync(cancellationToken);
        var applicationUsers = await _context.ApplicationUserAccess
            .Where(userAccess => userAccess.ApplicationId == application.Id)
            .ToListAsync(cancellationToken);
        var applicationRoles = await _context.ApplicationRoles
            .Where(role => role.ApplicationId == application.Id)
            .ToListAsync(cancellationToken);
        var clientCredentials = await _context.ApplicationClientCredentials
                .Where(x => x.ApplicationId == application.Id)
                .ToListAsync(cancellationToken);
        var refreshTokens = await _context.RefreshTokens
            .Where(token => token.ApplicationId == application.Id)
            .ToListAsync(cancellationToken);

        _context.ApplicationUserRoles.RemoveRange(applicationUserRoles);
        _context.ApplicationUserAccess.RemoveRange(applicationUsers);
        _context.ApplicationRoles.RemoveRange(applicationRoles);
        _context.RefreshTokens.RemoveRange(refreshTokens);
        _context.ApplicationClientCredentials.RemoveRange(clientCredentials);
        _context.Applications.Remove(application);
    }
}