using Authentication.Application.Abstractions.Identity;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.AssignRole;

public sealed record AssignRoleCommand(
    Guid UserId,
    UserRole Role) : IRequest<Result<AssignRoleResponse>>, IUnitOfWorkCommand;
