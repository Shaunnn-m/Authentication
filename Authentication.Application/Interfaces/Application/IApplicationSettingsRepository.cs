
using Authentication.Domain.Applications;

namespace Authentication.Application.Interfaces.Application;

public interface IApplicationSettingsRepository
{
    Task AddAsync(
        ApplicationSettings settings,
        CancellationToken cancellationToken = default);

    Task<ApplicationSettings?> GetByApplicationIdAsync(
        Guid applicationId,
        CancellationToken cancellationToken = default);
}