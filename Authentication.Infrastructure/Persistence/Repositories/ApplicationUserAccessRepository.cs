using Authentication.Application.Interfaces.Applications;
using Authentication.Domain.Applications;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence.Repositories;

public sealed class ApplicationUserAccessRepository
    : IApplicationUserAccessRepository
{
    private readonly AuthenticationDbContext _context;

    public ApplicationUserAccessRepository(
        AuthenticationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        ApplicationUserAccess access,
        CancellationToken cancellationToken = default)
    {
        await _context.ApplicationUserAccess.AddAsync(
            access,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid applicationId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ApplicationUserAccess
            .AnyAsync(
                x =>
                    x.ApplicationId == applicationId &&
                    x.UserId == userId &&
                    x.IsActive,
                cancellationToken);
    }

    public async Task<ApplicationUserAccess?> GetAsync(
        Guid applicationId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ApplicationUserAccess
            .FirstOrDefaultAsync(
                x =>
                    x.ApplicationId == applicationId &&
                    x.UserId == userId,
                cancellationToken);
    }
}