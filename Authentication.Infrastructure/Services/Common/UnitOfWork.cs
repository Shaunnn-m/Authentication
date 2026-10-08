using Authentication.Application.Interfaces.Common;
using Authentication.Infrastructure.Persistence;

namespace Authentication.Infrastructure.Services.Common;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AuthenticationDbContext _dbContext;

    public UnitOfWork(AuthenticationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}