using Authentication.Domain.Applications;

namespace Authentication.Application.Interfaces.Applications;

public interface IApplicationUserRoleRepository
{
    Task AddAsync(
        ApplicationUserRole userRole,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid applicationId,
        Guid userId,
        Guid roleId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApplicationRole>> GetRolesAsync(
        Guid applicationId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task RemoveAsync(
        ApplicationUserRole userRole,
        CancellationToken cancellationToken = default);
}