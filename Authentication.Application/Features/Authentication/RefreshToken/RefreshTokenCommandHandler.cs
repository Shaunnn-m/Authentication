using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
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
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationUserAccessRepository _applicationUserAccessRepository;
    private readonly IApplicationUserRoleRepository _applicationUserRoleRepository;

    public RefreshTokenCommandHandler(
        IRefreshTokenService refreshTokenService,
        IUserService userService,
        ITokenService tokenService,
        IApplicationRepository applicationRepository,
        IApplicationUserAccessRepository applicationUserAccessRepository,
        IApplicationUserRoleRepository applicationUserRoleRepository)
    {
        _refreshTokenService = refreshTokenService;
        _userService = userService;
        _tokenService = tokenService;
        _applicationRepository = applicationRepository;
        _applicationUserAccessRepository = applicationUserAccessRepository;
        _applicationUserRoleRepository = applicationUserRoleRepository;
    }

    public async Task<Result<RefreshTokenResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _refreshTokenService.ValidateAsync(
            request.RefreshToken,
            cancellationToken);

        if (validationResult.IsFailure || validationResult.Value is null)
        {

            return Result<RefreshTokenResponse>.Failure(
                UserMessages.InvalidRefreshToken);
        }

        var userId = validationResult.Value.UserId;

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

        var userEmail = userResult.Value.Email;
        var applicationId = validationResult.Value.ApplicationId;
        IReadOnlyList<string> roles;

        if (applicationId.HasValue)
        {
            var application = await _applicationRepository.GetByAppplicationId(
                applicationId.Value,
                cancellationToken);

            if (application is null)
            {
                return Result<RefreshTokenResponse>.Failure(
                    ApplicationMessages.NotFound);
            }

            if (!application.IsActive)
            {
                return Result<RefreshTokenResponse>.Failure(
                    ApplicationMessages.Inactive);
            }

            var hasApplicationAccess =
                await _applicationUserAccessRepository.ExistsAsync(
                    application.Id,
                    userId,
                    cancellationToken);

            if (!hasApplicationAccess)
            {
                return Result<RefreshTokenResponse>.Failure(
                    ApplicationMessages.UserAccessRequired);
            }

            var applicationRoles =
                await _applicationUserRoleRepository.GetRolesAsync(
                    application.Id,
                    userId,
                    cancellationToken);

            roles = applicationRoles
                .Select(role => role.Name)
                .ToList();
        }
        else
        {
            var rolesResult = await _userService.GetRolesAsync(
                userId,
                cancellationToken);

            if (rolesResult.IsFailure)
            {
                return Result<RefreshTokenResponse>.Failure(
                    rolesResult.Error!);
            }

            roles = rolesResult.Value ?? Array.Empty<string>();
        }

        var accessTokenResult = _tokenService.GenerateAccessToken(
            userId,
            userEmail,
            roles,
            applicationId);

        return Result<RefreshTokenResponse>.Success(
            new RefreshTokenResponse(
                accessTokenResult.Token,
                request.RefreshToken,
                accessTokenResult.ExpiresAt));
    }
}
