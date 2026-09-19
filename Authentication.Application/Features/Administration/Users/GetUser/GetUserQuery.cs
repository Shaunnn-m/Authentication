using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.GetUser;

public sealed record GetUserQuery(
    Guid UserId)
    : IRequest<Result<GetUserResponse>>;
