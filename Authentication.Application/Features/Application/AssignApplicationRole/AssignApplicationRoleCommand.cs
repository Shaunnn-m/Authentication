using Authentication.Application.Abstractions.Results.Application;
using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Applications.AssignApplicationRole;

public sealed record AssignApplicationRoleCommand(
    Guid ApplicationId,
    Guid UserId,
    Guid RoleId)
    : IRequest<Result<AssignApplicationRoleResult>>;