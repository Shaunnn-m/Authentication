using Authentication.Domain.Applications;

namespace Authentication.Application.Interfaces.Applications;

public interface IApplicationClientCredentialRepository
{
    Task AddAsync(
        ApplicationClientCredential credential,
        CancellationToken cancellationToken = default);

    Task<ApplicationClientCredential?> GetByIdAsync(
        Guid credentialId,
        CancellationToken cancellationToken = default);

    Task<ApplicationClientCredential?> GetByIdAndApplicationIdAsync(
        Guid credentialId,
        Guid applicationId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ApplicationClientCredential>> GetByApplicationIdAsync(
        Guid applicationId,
        CancellationToken cancellationToken = default);
}
