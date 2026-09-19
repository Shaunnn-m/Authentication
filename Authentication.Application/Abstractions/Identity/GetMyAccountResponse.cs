namespace Authentication.Application.Abstractions.Identity;

public sealed record UserAccountDetails(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    bool EmailConfirmed,
    IReadOnlyList<string> Roles);