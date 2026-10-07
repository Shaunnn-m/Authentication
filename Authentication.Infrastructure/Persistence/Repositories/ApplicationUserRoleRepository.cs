using Authentication.Application.Interfaces.Applications;
using Authentication.Domain.Applications;
using Authentication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Repositories;

public sealed class ApplicationUserRoleRepository
    : IApplicationUserRoleRepository
{
    private readonly AuthenticationDbContext _dbContext;

    public ApplicationUserRoleRepository(
        AuthenticationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        ApplicationUserRole userRole,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.ApplicationUserRoles.AddAsync(
            userRole,
            cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Guid applicationId,
        Guid userId,
        Guid roleId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ApplicationUserRoles
            .AnyAsync(
                x =>
                    x.ApplicationId == applicationId &&
                    x.UserId == userId &&
                    x.RoleId == roleId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<ApplicationRole>> GetRolesAsync(
        Guid applicationId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.ApplicationUserRoles
            .Where(x =>
                x.ApplicationId == applicationId &&
                x.UserId == userId)
            .Join(
                _dbContext.ApplicationRoles,
                userRole => userRole.RoleId,
                role => role.Id,
                (userRole, role) => role)
            .Where(role => role.IsActive)
            .OrderBy(role => role.Name)
            .ToListAsync(cancellationToken);
    }

    public Task RemoveAsync(
        ApplicationUserRole userRole,
        CancellationToken cancellationToken = default)
    {
        _dbContext.ApplicationUserRoles.Remove(userRole);

        return Task.CompletedTask;
    }
}