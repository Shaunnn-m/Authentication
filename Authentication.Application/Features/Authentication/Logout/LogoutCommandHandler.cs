using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using MediatR;

namespace Authentication.Application.Features.Authentication.Logout;

public sealed class LogoutCommandHandler
    : IRequestHandler<LogoutCommand, Result<LogoutResponse>>
{
    private readonly IRefreshTokenService _refreshTokenService;

    public LogoutCommandHandler(
        IRefreshTokenService refreshTokenService)
    {
        _refreshTokenService = refreshTokenService;
    }

    public async Task<Result<LogoutResponse>> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        await _refreshTokenService.RevokeAsync(
            request.RefreshToken,
            cancellationToken);

        return Result<LogoutResponse>.Success(
            new LogoutResponse(
                "Logged out successfully."));
    }
}
