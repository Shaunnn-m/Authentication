using Authentication.Application.Abstractions.Results.Application;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Application.RegisterApplicationRole
{
    public sealed record RegisterApplicationRoleCommand(
    Guid ApplicationId,
    string Name)
    : IRequest<Result<RegisterApplicationRoleResult>>, IUnitOfWorkCommand;
}
