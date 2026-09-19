using Authentication.Application.Abstractions.Email;
using Authentication.Application.Abstractions.Results.Email;
using Authentication.Application.Common.Results;

namespace Authentication.Application.Interfaces.Email;

public interface IEmailService
{
    Task SendAsync(
        EmailMessage email,
        CancellationToken cancellationToken = default);

    Task<Result<EmailConfirmationResult?>> HandleAsync(
        Guid userId,
        string firstName,
        string email,
        CancellationToken cancellationToken = default);

    Task<Result<EmailChangeResult?>> RequestChangeAsync(
    Guid userId,
    string newEmail,
    CancellationToken cancellationToken = default);

    Task<Result<bool>> ConfirmChangeAsync(
        Guid userId,
        string newEmail,
        string token,
        CancellationToken cancellationToken = default);
}