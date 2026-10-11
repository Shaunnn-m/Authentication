using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Features.Authentication.Login;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Application.LoginApplicationUser;

public sealed class LoginApplicationUserCommandHandler
    : IRequestHandler<LoginApplicationUserCommand, Result<LoginResponse>>
{
    private readonly IPasswordService _passwordService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationUserAccessRepository _applicationUserAccessRepository;
    private readonly IApplicationUserRoleRepository _applicationUserRoleRepository;

    public LoginApplicationUserCommandHandler(
        IPasswordService passwordService,
        IUserService userService,
        ITokenService tokenService,
        IRefreshTokenService refreshTokenService,
        IApplicationRepository applicationRepository,
        IApplicationUserAccessRepository applicationUserAccessRepository,
        IApplicationUserRoleRepository applicationUserRoleRepository)
    {
        _passwordService = passwordService;
        _userService = userService;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
        _applicationRepository = applicationRepository;
        _applicationUserAccessRepository = applicationUserAccessRepository;
        _applicationUserRoleRepository = applicationUserRoleRepository;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginApplicationUserCommand request,
        CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetByAppplicationId(
            request.ApplicationId,
            cancellationToken);

        if (application is null)
        {
            return Result<LoginResponse>.Failure(
                ApplicationMessages.NotFound);
        }

        if (!application.IsActive)
        {
            return Result<LoginResponse>.Failure(
                ApplicationMessages.Inactive);
        }

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

        var hasApplicationAccess =
            await _applicationUserAccessRepository.ExistsAsync(
                application.Id,
                userId,
                cancellationToken);

        if (!hasApplicationAccess)
        {
            return Result<LoginResponse>.Failure(
                UserMessages.InvalidCredentials);
        }

        var emailConfirmedResult = await _userService.IsEmailConfirmedAsync(
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

        var activeResult = await _userService.IsActiveAsync(
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

        var applicationRoles =
            await _applicationUserRoleRepository.GetRolesAsync(
                application.Id,
                userId,
                cancellationToken);

        var accessTokenResult = _tokenService.GenerateAccessToken(
            userId,
            request.Email,
            applicationRoles.Select(role => role.Name),
            application.Id);

        var refreshTokenResult = await _refreshTokenService.CreateAsync(
            userId,
            application.Id,
            cancellationToken: cancellationToken);

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
