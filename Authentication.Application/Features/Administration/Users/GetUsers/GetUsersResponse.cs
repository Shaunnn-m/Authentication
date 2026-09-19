namespace Authentication.Application.Features.Administration.Users.GetUsers;

public sealed record GetUsersResponse(
    IReadOnlyList<GetUserItemResponse> Users,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);

public sealed record GetUserItemResponse(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    bool EmailConfirmed,
    IReadOnlyList<string> Roles);
