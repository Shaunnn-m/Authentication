using Authentication.Application.Abstractions.Identity;

namespace Authentication.Application.Features.Administration.Users.AssignRole;

public sealed record AssignRoleResponse(
    Guid UserId,
    UserRole Role,
    string Message);
