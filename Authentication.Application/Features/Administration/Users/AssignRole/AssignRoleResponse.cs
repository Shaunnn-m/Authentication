namespace Authentication.Application.Features.Administration.Users.AssignRole;

public sealed record AssignRoleResponse(
    Guid UserId,
    string Role,
    string Message);
