using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Authentication.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<Result<LoginResponse>>, IUnitOfWorkCommand;
