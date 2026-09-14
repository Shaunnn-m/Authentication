using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.Logout;

public sealed record LogoutCommand(
    string RefreshToken) : IRequest<Result<LogoutResponse>>;
