namespace Authentication.Application.Abstractions.Identity;

public sealed record AdminUserList(
    IReadOnlyList<AdminUserDetails> Users,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);

public sealed record AdminUserDetails(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    bool EmailConfirmed,
    IReadOnlyList<string> Roles);