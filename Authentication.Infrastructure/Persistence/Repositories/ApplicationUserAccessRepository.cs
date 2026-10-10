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

    public async Task<IReadOnlyList<string>> GetActiveApplicationNamesByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await (
            from access in _context.ApplicationUserAccess
            join application in _context.Applications
                on access.ApplicationId equals application.Id
            where access.UserId == userId &&
                  access.IsActive &&
                  application.IsActive
            select application.Name)
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}