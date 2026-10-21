using Authentication.Application.Abstractions.Email;
using Authentication.Application.Common.Results;

namespace Authentication.Application.Interfaces.Authentication;

public interface IPasswordService
{
    Task<Result<Guid>> ValidateCredentialsAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<Result<PasswordResetResult?>> RequestAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<Result<string>> GenerateResetTokenAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Result<bool>> ResetPasswordAsync(
        Guid userId,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default);

    Task<Result<bool>> ChangePasswordAsync(
        Guid userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default);
}