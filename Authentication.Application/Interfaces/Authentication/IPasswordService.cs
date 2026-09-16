using Authentication.Application.Abstractions.Email;
using Authentication.Application.Common.Results;

namespace Authentication.Application.Interfaces.Authentication;

public interface IPasswordService
{
    Task<Result<PasswordResetResult?>> RequestAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<Result<bool>> ResetAsync(
        Guid userId,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default);

}