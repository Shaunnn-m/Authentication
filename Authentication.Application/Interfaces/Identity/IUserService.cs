using Authentication.Application.Common.Results;

namespace Authentication.Application.Interfaces.Identity;

public interface IUserService
{
    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<Result<Guid>> CreateAsync(
        string firstName,
        string lastName,
        string email,
        string password,
        CancellationToken cancellationToken = default);
}