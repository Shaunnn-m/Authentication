
namespace Authentication.Application.Abstractions.Email;
public sealed record PasswordResetResult(
    Guid UserId,
    string Email,
    string FirstName,
    string PasswordResetLink);