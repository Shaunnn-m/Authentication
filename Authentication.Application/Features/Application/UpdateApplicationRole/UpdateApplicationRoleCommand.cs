using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Application.UpdateApplicationRole;

public sealed record UpdateApplicationRoleCommand(
    Guid ApplicationId,
    Guid RoleId,
    string Name)
    : IRequest<Result<ApplicationRoleResponse>>, IUnitOfWorkCommand;
