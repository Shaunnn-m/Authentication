using Authentication.Domain.Applications;

namespace Authentication.Application.Interfaces.Applications;

public interface IApplicationUserAccessRepository
{
    Task AddAsync(
        ApplicationUserAccess access,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid applicationId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<ApplicationUserAccess?> GetAsync(
        Guid applicationId,
        Guid userId,
        CancellationToken cancellationToken = default);
}