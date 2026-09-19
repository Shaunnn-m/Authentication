using Authentication.Application.Abstractions.Identity;
using Authentication.Application.Common.Results;

namespace Authentication.Application.Interfaces.Identity;

public interface IAdminUserService
{
    Task<Result<AdminUserList>> GetUsersAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Result<AdminUserDetails>> GetUserAsync(
    Guid userId,
    CancellationToken cancellationToken = default);
}