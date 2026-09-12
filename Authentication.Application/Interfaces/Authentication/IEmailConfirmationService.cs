using Authentication.Application.Abstractions.Email;
using Authentication.Application.Common.Results;


namespace Authentication.Application.Interfaces.Authentication
{
    public interface IEmailConfirmationService
    {
        Task<Result<EmailConfirmationResult>> HandleAsync(
            Guid userId,
            string firstName,
            string email,
            CancellationToken cancellationToken = default);
    }
}
