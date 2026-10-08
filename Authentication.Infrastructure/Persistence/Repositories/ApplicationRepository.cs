using Authentication.Application.Interfaces.Applications;
using ApplicationEntity = Authentication.Domain.Applications.Application;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence.Repositories;

public sealed class ApplicationRepository : IApplicationRepository
{
    private readonly AuthenticationDbContext _context;

    public ApplicationRepository(AuthenticationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        ApplicationEntity application,
        CancellationToken cancellationToken = default)
    {
        await _context.Applications.AddAsync(
            application,
            cancellationToken);
    }

    public async Task<ApplicationEntity?> GetByClientIdAsync(
        string clientId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Applications
            .FirstOrDefaultAsync(
                application => application.ClientId == clientId,
                cancellationToken);
    }

    public async Task<ApplicationEntity?> GetByAppplicationId(
        Guid ApplicationId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Applications
            .FirstOrDefaultAsync(
                    application => application.Id == ApplicationId,
                    cancellationToken);
    }
}