using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.DeactivateUser;

public sealed record DeactivateUserCommand(
    Guid UserId)
    : IRequest<Result<DeactivateUserResponse>>;
