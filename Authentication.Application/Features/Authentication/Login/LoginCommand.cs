using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<Result<LoginResponse>>;
