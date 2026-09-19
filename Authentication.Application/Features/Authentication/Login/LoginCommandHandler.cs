using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authentication.Login;

public sealed class LoginCommandHandler
    : IRequestHandler<
        LoginCommand,
        Result<LoginResponse>>
{
    private readonly IPasswordService _passwordService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public LoginCommandHandler(
        IPasswordService passwordService,
        IUserService userService,
        ITokenService tokenService,
        IRefreshTokenService refreshTokenService)
    {
        _passwordService = passwordService;
        _userService = userService;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Validate credentials
        var credentialsResult =
            await _passwordService.ValidateCredentialsAsync(
                request.Email,
                request.Password,
                cancellationToken);

        if (credentialsResult.IsFailure)
        {
            return Result<LoginResponse>.Failure(
                credentialsResult.Error!);
        }

        var userId = credentialsResult.Value;

        // 2. Verify email confirmation
        var emailConfirmedResult =
            await _userService.IsEmailConfirmedAsync(
                userId,
                cancellationToken);

        if (emailConfirmedResult.IsFailure)
        {
            return Result<LoginResponse>.Failure(
                emailConfirmedResult.Error!);
        }

        if (!emailConfirmedResult.Value)
        {
            return Result<LoginResponse>.Failure(
                UserMessages.EmailNotConfirmed);
        }

        // 3. Verify account is active
        var activeResult =
            await _userService.IsActiveAsync(
                userId,
                cancellationToken);

        if (activeResult.IsFailure)
        {
            return Result<LoginResponse>.Failure(
                activeResult.Error!);
        }

        if (!activeResult.Value)
        {
            return Result<LoginResponse>.Failure(
                UserMessages.AccountInactive);
        }

        // 4. Get roles
        var rolesResult =
            await _userService.GetRolesAsync(
                userId,
                cancellationToken);

        if (rolesResult.IsFailure)
        {
            return Result<LoginResponse>.Failure(
                rolesResult.Error!);
        }

        // 5. Generate access token
        var accessTokenResult = _tokenService.GenerateAccessToken(
            userId,
            request.Email,
            rolesResult.Value ?? Array.Empty<string>());

        // 6. Generate refresh token
        var refreshTokenResult =
            await _refreshTokenService.CreateAsync(
                userId,
                cancellationToken);

        if (refreshTokenResult.IsFailure)
        {
            return Result<LoginResponse>.Failure(
                refreshTokenResult.Error!);
        }

        return Result<LoginResponse>.Success(
            new LoginResponse(
                accessTokenResult.Token,
                refreshTokenResult.Value!,
                accessTokenResult.ExpiresAt));
    }
}