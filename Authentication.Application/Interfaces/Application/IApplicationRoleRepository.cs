using Authentication.Domain.Applications;

namespace Authentication.Application.Interfaces.Application;

public interface IApplicationRoleRepository
{
    Task AddAsync(
        ApplicationRole role,
        CancellationToken cancellationToken = default);

    Task<ApplicationRole?> GetByIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default);

    Task<ApplicationRole?> GetByNameAsync(
        Guid applicationId,
        string name,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApplicationRole>> GetByApplicationIdAsync(
        Guid applicationId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid applicationId,
        string name,
        CancellationToken cancellationToken = default);
}