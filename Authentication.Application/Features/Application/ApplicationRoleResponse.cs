namespace Authentication.Application.Features.Application;

public sealed record ApplicationRoleResponse(
    Guid RoleId,
    Guid ApplicationId,
    string Name,
    bool IsActive);
