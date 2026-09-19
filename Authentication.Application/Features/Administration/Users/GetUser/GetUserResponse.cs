namespace Authentication.Application.Features.Administration.Users.GetUser;

public sealed record GetUserResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    bool EmailConfirmed,
    IReadOnlyList<string> Roles);
