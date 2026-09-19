using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.AssignRole;

public sealed record AssignRoleCommand(
    Guid UserId,
    string Role) : IRequest<Result<AssignRoleResponse>>;
