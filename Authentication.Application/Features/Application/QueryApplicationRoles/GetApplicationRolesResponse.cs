namespace Authentication.Application.Features.Application.QueryApplicationRoles;

public sealed record GetApplicationRolesResponse(
    IReadOnlyList<ApplicationRoleResponse> Roles);
