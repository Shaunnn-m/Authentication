namespace Authentication.Application.Interfaces.Identity;

public interface IUserService
{
    Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<(bool Succeeded, Guid UserId, IEnumerable<string> Errors)> CreateAsync(
        string firstName,
        string lastName,
        string email,
        string password,
        CancellationToken cancellationToken = default);
}