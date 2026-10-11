using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Application.QueryApplicationRoles;

public sealed record GetApplicationRolesQuery
    : IRequest<Result<GetApplicationRolesResponse>>;
