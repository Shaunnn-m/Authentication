using Authentication.Application.Interfaces.Application;
using Authentication.Domain.Applications;

namespace Authentication.Infrastructure.Persistence.Repositories;

public sealed class ApplicationSettingsRepository
    : IApplicationSettingsRepository
{
    private readonly AuthenticationDbContext _dbContext;

    public ApplicationSettingsRepository(AuthenticationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        ApplicationSettings settings,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.ApplicationSettings.AddAsync(
            settings,
            cancellationToken);
    }
}
