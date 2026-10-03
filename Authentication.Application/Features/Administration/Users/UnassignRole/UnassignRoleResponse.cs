using Authentication.Application.Abstractions.Identity;

namespace Authentication.Application.Features.Administration.Users.UnassignRole;

public sealed record UnassignRoleResponse(
    Guid UserId,
    UserRole Role,
    string message);