using Authentication.Application.Abstractions.Identity;
using Authentication.Application.Common.Results;
using MediatR;


namespace Authentication.Application.Features.Administration.Users.UnassignRole;

public sealed record UnassignRoleCommand(
    Guid UserId,
    UserRole Role) : IRequest<Result<UnassignRoleResponse>>;