using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Application.QueryApplicationRoles;

public sealed record GetApplicationRolesQuery(
    Guid ApplicationId)
    : IRequest<Result<GetApplicationRolesResponse>>;
