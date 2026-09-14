using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.RefreshToken;

public sealed record RefreshTokenCommand(
    string RefreshToken) : IRequest<Result<RefreshTokenResponse>>;
