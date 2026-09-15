using Authentication.Application.Abstractions.Email;
using Authentication.Application.Common.Results;

namespace Authentication.Application.Interfaces.Authentication;

public interface IPasswordResetService
{
    Task<Result<PasswordResetResult?>> RequestAsync(
        string email,
        CancellationToken cancellationToken = default);
}