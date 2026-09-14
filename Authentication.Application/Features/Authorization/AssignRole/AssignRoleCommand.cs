using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authorization.AssignRole;

public sealed record AssignRoleCommand(
    Guid UserId,
    string Role) : IRequest<Result<AssignRoleResponse>>;
