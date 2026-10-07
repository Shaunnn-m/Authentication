using Authentication.Application.Abstractions.Results.Application;
using MediatR;

namespace Authentication.Application.Features.Application.RegisterApplicationRole
{
    public sealed record RegisterApplicationRoleCommand(
    Guid ApplicationId,
    string Name)
    : IRequest<RegisterApplicationRoleResult>;
}
