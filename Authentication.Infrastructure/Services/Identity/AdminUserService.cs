using Authentication.Application.Abstractions.Identity;
using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using Authentication.Infrastructure.Identity;
using Authentication.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Services.Identity;

public sealed class AdminUserService : IAdminUserService
{
    private readonly AuthenticationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminUserService(
        AuthenticationDbContext dbContext,
        UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<Result<AdminUserList>> GetUsersAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _userManager.Users
            .AsNoTracking()
            .OrderBy(user => user.Email);

        var totalCount = await query.CountAsync(
            cancellationToken);

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)pageSize);

        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var userIds = users
            .Select(user => user.Id)
            .ToList();

        var roles = await (
            from userRole in _dbContext.UserRoles.AsNoTracking()
            join role in _dbContext.Roles.AsNoTracking()
                on userRole.RoleId equals role.Id
            where userIds.Contains(userRole.UserId)
            select new
            {
                userRole.UserId,
                RoleName = role.Name!
            })
            .ToListAsync(cancellationToken);

        var roleLookup = roles
            .GroupBy(role => role.UserId)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<string>)group
                    .Select(role => role.RoleName)
                    .OrderBy(role => role)
                    .ToList());

        var userDetails = users
            .Select(user => new AdminUserDetails(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email!,
                user.IsActive,
                user.EmailConfirmed,
                roleLookup.TryGetValue(
                    user.Id,
                    out var userRoles)
                    ? userRoles
                    : Array.Empty<string>()))
            .ToList();

        return Result<AdminUserList>.Success(
            new AdminUserList(
                userDetails,
                page,
                pageSize,
                totalCount,
                totalPages));
    }

    public async Task<Result<AdminUserDetails>> GetUserAsync(
        UserIdentifierType identifierType,
        string identifier,
        CancellationToken cancellationToken = default)
    {
        ApplicationUser? user;

        if (identifierType == UserIdentifierType.Id)
        {
            var userId = Guid.Parse(identifier);
            user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    user => user.Id == userId,
                    cancellationToken);
        }
        else
        {
            var normalizedEmail = identifier
                .Trim()
                .ToUpperInvariant();

            user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    user => user.NormalizedEmail == normalizedEmail,
                    cancellationToken);
        }

        if (user == null)
        {
            return Result<AdminUserDetails>.Failure(
            UserMessages.NotFound);
        }

        var roles = await (
            from userRole in _dbContext.UserRoles.AsNoTracking()
            join role in _dbContext.Roles.AsNoTracking()
                on userRole.RoleId equals role.Id
            where userRole.UserId == user.Id
            select role.Name!
            )
            .ToListAsync(cancellationToken);

        var userDetails = new AdminUserDetails(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email!,
            user.IsActive,
            user.EmailConfirmed,
            roles);

        return Result<AdminUserDetails>.Success(userDetails);
    }

    public async Task<Result<bool>> ActivateAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return Result<bool>.Failure(
                UserMessages.NotFound);
        }

        if (user.IsActive)
        {
            return Result<bool>.Success(true);
        }

        user.IsActive = true;
        
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return Result<bool>.Failure(
                UserMessages.FailedToActivateUser);
        }
        
        return Result<bool>.Success(true);
    }
}