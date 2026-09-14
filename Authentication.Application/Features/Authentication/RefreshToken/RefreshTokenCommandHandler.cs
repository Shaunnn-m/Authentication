using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authentication.RefreshToken;

public sealed class RefreshTokenCommandHandler
    : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(
        IRefreshTokenService refreshTokenService,
        IUserService userService,
        ITokenService tokenService)
    {
        _refreshTokenService = refreshTokenService;
        _userService = userService;
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
            return Result<RefreshTokenResponse>.Failure(
                UserMessages.InvalidRefreshToken);
        }

        var userId = validationResult.Value;

        var userResult = await _userService.GetByIdAsync(
            userId,
            cancellationToken);

        if (userResult.IsFailure)
        {
            return Result<RefreshTokenResponse>.Failure(
                UserMessages.NotFound);
        }

        var activeResult = await _userService.IsActiveAsync(
            userId,
            cancellationToken);

        if (activeResult.IsFailure)
        {
            return Result<RefreshTokenResponse>.Failure(
                activeResult.Error!);
        }

        if (!activeResult.Value)
        {
            return Result<RefreshTokenResponse>.Failure(
                UserMessages.AccountInactive);
        }

        var rolesResult = await _userService.GetRolesAsync(
            userId,
            cancellationToken);

        if (rolesResult.IsFailure)
        {
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
