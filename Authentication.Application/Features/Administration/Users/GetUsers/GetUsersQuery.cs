using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.GetUsers;

public sealed record GetUsersQuery(
    int Page = 1,
    int PageSize = 10)
    : IRequest<Result<GetUsersResponse>>;
