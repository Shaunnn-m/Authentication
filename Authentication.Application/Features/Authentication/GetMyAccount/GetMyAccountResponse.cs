namespace Authentication.Application.Features.Authentication.GetMyAccount;

public sealed record GetMyAccountResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    bool EmailConfirmed,
    IReadOnlyList<string> Roles);
