namespace Authentication.Application.Abstractions.Results.Application;

public sealed record AssignApplicationRoleResult(
    Guid ApplicationId,
    Guid UserId,
    Guid RoleId,
    DateTime CreatedAt);