using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.DeactivateUser;

public sealed record DeactivateUserCommand(
    Guid UserId)
    : IRequest<Result<DeactivateUserResponse>>, IUnitOfWorkCommand;
