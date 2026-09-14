namespace Authentication.Application.Features.Authorization.AssignRole;

public sealed record AssignRoleResponse(
    Guid UserId,
    string Role,
    string Message);
