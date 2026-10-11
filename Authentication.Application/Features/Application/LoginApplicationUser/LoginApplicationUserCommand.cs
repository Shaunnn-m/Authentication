using Authentication.Application.Common.Results;
using Authentication.Application.Features.Authentication.Login;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Application.LoginApplicationUser;

public sealed record LoginApplicationUserCommand(
    Guid ApplicationId,
    string Email,
    string Password)
    : IRequest<Result<LoginResponse>>, IUnitOfWorkCommand;
