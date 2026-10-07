using Authentication.Application.Interfaces.Application;
using Authentication.Domain.Applications;
using Authentication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Services.Application
{
    public sealed class ApplicationRoleRepository
    : IApplicationRoleRepository
    {
        private readonly AuthenticationDbContext _dbContext;

        public ApplicationRoleRepository(
            AuthenticationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(
            ApplicationRole role,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.ApplicationRoles.AddAsync(
                role,
                cancellationToken);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApplicationRole?> GetByIdAsync(
            Guid roleId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.ApplicationRoles
                .FirstOrDefaultAsync(
                    role => role.Id == roleId,
                    cancellationToken);
        }

        public async Task<ApplicationRole?> GetByNameAsync(
            Guid applicationId,
            string name,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.ApplicationRoles
                .FirstOrDefaultAsync(
                    role =>
                        role.ApplicationId == applicationId &&
                        role.Name == name,
                    cancellationToken);
        }

        public async Task<IReadOnlyList<ApplicationRole>>
            GetByApplicationIdAsync(
                Guid applicationId,
                CancellationToken cancellationToken = default)
        {
            return await _dbContext.ApplicationRoles
                .Where(role =>
                    role.ApplicationId == applicationId)
                .OrderBy(role => role.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(
            Guid applicationId,
            string name,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.ApplicationRoles
                .AnyAsync(
                    role =>
                        role.ApplicationId == applicationId &&
                        role.Name == name,
                    cancellationToken);
        }
    }
}
