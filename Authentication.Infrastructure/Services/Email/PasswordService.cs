using Authentication.Application.Abstractions.Email;
using Authentication.Application.Abstractions.Results;
using Authentication.Application.Common.Authentication;
using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Interfaces.Identity;
using Authentication.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Authentication.Infrastructure.Services.Identity;

public sealed class PasswordService : IPasswordService
{
    private readonly IUserService _userService;
    private readonly IAuthenticationLinkService _linkService;
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly AuthenticationEmailOptions _emailOptions;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly UserManager<ApplicationUser> _userManager;

    public PasswordService(
        IUserService userService,
        IAuthenticationLinkService linkService,
        IEmailService emailService,
        IEmailTemplateService emailTemplateService,
        IOptions<AuthenticationEmailOptions> emailOptions,
        IRefreshTokenService refreshTokenService,
        UserManager<ApplicationUser> userManager)
    {
        _userService = userService;
        _linkService = linkService;
        _emailService = emailService;
        _emailTemplateService = emailTemplateService;
        _emailOptions = emailOptions.Value;
        _refreshTokenService = refreshTokenService;
        _userManager = userManager;
    }

    public async Task<Result<Guid>> ValidateCredentialsAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return Result<Guid>.Failure(UserMessages.InvalidCredentials);
        }

        var passwordValid = await _userManager.CheckPasswordAsync(
            user,
            password);

        if (!passwordValid)
        {
            return Result<Guid>.Failure(UserMessages.InvalidCredentials);
        }

        return Result<Guid>.Success(user.Id);
    }

    public async Task<Result<PasswordResetResult?>> RequestAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        if (!_emailOptions.Enabled)
        {
            return Result<PasswordResetResult?>.Success(null);
        }

        var userResult = await _userService.GetByEmailAsync(
            email,
            cancellationToken);

        if (userResult.IsFailure)
        {
            return Result<PasswordResetResult?>.Success(null);
        }

        var tokenResult = await GenerateResetTokenAsync(
            userResult.Value.UserId,
            cancellationToken);

        if (tokenResult.IsFailure)
        {
            return Result<PasswordResetResult?>.Success(null);
        }

        var resetLink = _linkService.CreatePasswordResetLink(
            userResult.Value.UserId,
            tokenResult.Value);

        var result = new PasswordResetResult(
            userResult.Value.UserId,
            userResult.Value.Email,
            userResult.Value.FirstName,
            resetLink);

        if (_emailOptions.Mode == AuthenticationEmailMode.AuthenticationService)
        {
            var body = _emailTemplateService.RenderPasswordReset(
                result.FirstName,
                result.PasswordResetLink);

            var emailMessage = new EmailMessage(
                result.Email,
                "Reset your password",
                body);

            await _emailService.SendAsync(
                emailMessage,
                cancellationToken);

            return Result<PasswordResetResult?>.Success(null);
        }

        return Result<PasswordResetResult?>.Success(result);
    }

    public async Task<Result<string>> GenerateResetTokenAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result<string>.Failure(UserMessages.NotFound);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return Result<string>.Success(token);
    }

    public async Task<Result<bool>> ResetPasswordAsync(
        Guid userId,
        string token,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result<bool>.Failure(UserMessages.NotFound);
        }

        var result = await _userManager.ResetPasswordAsync(
            user,
            token,
            newPassword);

        if (!result.Succeeded)
        {
            return Result<bool>.Failure(UserMessages.PasswordResetFailed);
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> ChangePasswordAsync(
        Guid userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result<bool>.Failure(UserMessages.NotFound);
        }

        var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(
            user,
            currentPassword);

        if (!isCurrentPasswordValid)
        {
            return Result<bool>.Failure(UserMessages.InvalidCredentials);
        }

        var result = await _userManager.ChangePasswordAsync(
            user,
            currentPassword,
            newPassword);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(error => error.Description));

            return Result<bool>.Failure(
                new ResultError(
                    "User.ChangePasswordFailed",
                    errors,
                    ErrorType.Failure));
        }

        await _refreshTokenService.RevokeAllAsync(userId, cancellationToken);

        return Result<bool>.Success(true);
    }
}