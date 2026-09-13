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

    Task<Result<string>> GenerateEmailConfirmationTokenAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Result<string>> ConfirmEmailAsync(
        Guid userId,
        string token,
        CancellationToken cancellationToken = default);

    Task<Result<(Guid UserId, string FirstName, string Email)>> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<Result<bool>> IsEmailConfirmedAsync(
    Guid userId,
    CancellationToken cancellationToken = default);

    Task<Result<Guid>> ValidateCredentialsAsync(
    string email,
    string password,
    CancellationToken cancellationToken = default);

    Task<Result<bool>> IsActiveAsync(
    Guid userId,
    CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<string>>> GetRolesAsync(
    Guid userId,
    CancellationToken cancellationToken = default);
}