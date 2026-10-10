namespace Authentication.Application.Features.Applications.AssignApplicationRole;

public sealed record AssignApplicationRoleResponse(
    Guid ApplicationId,
    Guid UserId,
    Guid RoleId,
    DateTime CreatedAt);
