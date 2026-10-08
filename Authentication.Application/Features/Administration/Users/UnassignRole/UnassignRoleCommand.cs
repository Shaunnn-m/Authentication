using Authentication.Application.Abstractions.Identity;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;


namespace Authentication.Application.Features.Administration.Users.UnassignRole;

public sealed record UnassignRoleCommand(
    Guid UserId,
    UserRole Role) : IRequest<Result<UnassignRoleResponse>>, IUnitOfWorkCommand;