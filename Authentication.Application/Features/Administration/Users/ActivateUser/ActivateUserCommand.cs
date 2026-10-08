using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.ActivateUser;

public sealed record ActivateUserCommand(
    Guid UserId)
    : IRequest<Result<ActivateUserResponse>>, IUnitOfWorkCommand;
