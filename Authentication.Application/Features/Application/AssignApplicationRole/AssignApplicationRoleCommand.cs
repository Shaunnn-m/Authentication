using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Applications.AssignApplicationRole;

public sealed record AssignApplicationRoleCommand(
    Guid UserId,
    Guid RoleId)
    : IRequest<Result<AssignApplicationRoleResponse>>, IUnitOfWorkCommand;