using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Application.RegisterApplicationRole
{
    public sealed record RegisterApplicationRoleCommand(
    string Name)
    : IRequest<Result<RegisterApplicationRoleResponse>>, IUnitOfWorkCommand;
}
