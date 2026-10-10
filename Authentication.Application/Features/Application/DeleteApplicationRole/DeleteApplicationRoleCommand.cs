using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Application.DeleteApplicationRole;

public sealed record DeleteApplicationRoleCommand(
    Guid ApplicationId,
    Guid RoleId)
    : IRequest<Result<ApplicationRoleResponse>>, IUnitOfWorkCommand;
