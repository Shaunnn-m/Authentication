namespace Authentication.Application.Features.Application.RegisterApplicationRole;

public sealed record RegisterApplicationRoleResponse(
    Guid RoleId,
    Guid ApplicationId,
    string Name);
