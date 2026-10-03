using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Identity;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.Features.Authentication.RefreshToken;

public sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IRefreshTokenService refreshTokenService,
        IUserService userService,
        ILogger<RefreshTokenCommandHandler> logger,
        ITokenService tokenService)
    {
        _refreshTokenService = refreshTokenService;
        _userService = userService;
        _logger = logger;
        _tokenService = tokenService;
    }

    public async Task<Result<RefreshTokenResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _refreshTokenService.ValidateAsync(
            request.RefreshToken,
            cancellationToken);

        if (validationResult.IsFailure)
        {
            _logger.LogWarning("Invalid refresh token provided.");
            return Result<RefreshTokenResponse>.Failure(
                UserMessages.InvalidRefreshToken);
        }

        var userId = validationResult.Value;

        var userResult = await _userService.GetByIdAsync(
            userId,
            cancellationToken);

        if (userResult.IsFailure)
        {
            _logger.LogWarning("User not found.");
            return Result<RefreshTokenResponse>.Failure(
                UserMessages.NotFound);
        }

        var activeResult = await _userService.IsActiveAsync(
            userId,
            cancellationToken);

        if (activeResult.IsFailure)
        {
            _logger.LogWarning("Failed to check if user is active.");
            return Result<RefreshTokenResponse>.Failure(
                activeResult.Error!);
        }

        if (!activeResult.Value)
        {
            _logger.LogWarning("Inactive user attempted to refresh token.");
            return Result<RefreshTokenResponse>.Failure(
                UserMessages.AccountInactive);
        }

        var rolesResult = await _userService.GetRolesAsync(
            userId,
            cancellationToken);

        if (rolesResult.IsFailure)
        {
            _logger.LogWarning("Failed to retrieve user roles.");
            return Result<RefreshTokenResponse>.Failure(
                rolesResult.Error!);
        }

        var userEmail = userResult.Value.Email;

        var accessTokenResult = _tokenService.GenerateAccessToken(
            userId,
            userEmail,
            rolesResult.Value ?? Array.Empty<string>());

        return Result<RefreshTokenResponse>.Success(
            new RefreshTokenResponse(
                accessTokenResult.Token,
                request.RefreshToken,
                accessTokenResult.ExpiresAt));
    }
}
