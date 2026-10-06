using ApplicationEntity = Authentication.Domain.Applications.Application;

namespace Authentication.Application.Interfaces.Applications;

public interface IApplicationRepository
{
    Task AddAsync(
        ApplicationEntity application,
        CancellationToken cancellationToken = default);

    Task<ApplicationEntity?> GetByClientIdAsync(
        string clientId,
        CancellationToken cancellationToken = default);

    Task<ApplicationEntity?> GetByAppplicationId(
        Guid ApplicationId,
        CancellationToken cancellationToken = default);

}