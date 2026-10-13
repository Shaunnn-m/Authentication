using Authentication.Application.Interfaces.Applications;
using Authentication.Domain.Applications;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence.Repositories;

public sealed class ApplicationClientCredentialRepository
    : IApplicationClientCredentialRepository
{
    private readonly AuthenticationDbContext _context;

    public ApplicationClientCredentialRepository(
        AuthenticationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        ApplicationClientCredential credential,
        CancellationToken cancellationToken = default)
    {
        await _context.ApplicationClientCredentials.AddAsync(
            credential,
            cancellationToken);
    }

    public async Task<ApplicationClientCredential?> GetByIdAsync(
        Guid credentialId,
        CancellationToken cancellationToken = default)
    {
        return await _context.ApplicationClientCredentials
            .FirstOrDefaultAsync(
                x => x.Id == credentialId,
                cancellationToken);
    }

    public async Task<ApplicationClientCredential?>
        GetByIdAndApplicationIdAsync(
            Guid credentialId,
            Guid applicationId,
            CancellationToken cancellationToken = default)
    {
        return await _context.ApplicationClientCredentials
            .FirstOrDefaultAsync(
                x => x.Id == credentialId &&
                     x.ApplicationId == applicationId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<ApplicationClientCredential>>
        GetByApplicationIdAsync(
            Guid applicationId,
            CancellationToken cancellationToken = default)
    {
        return await _context.ApplicationClientCredentials
            .Where(x => x.ApplicationId == applicationId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
