using Authentication.Application.Abstractions.Email;

namespace Authentication.Application.Interfaces.Email;

public interface IEmailService
{
    Task SendAsync(
        EmailMessage email,
        CancellationToken cancellationToken = default);
}