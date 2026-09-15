using Authentication.Application.Abstractions.Email;
using Authentication.Application.Common.Authentication;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Interfaces.Identity;
using Microsoft.Extensions.Options;

namespace Authentication.Infrastructure.Services.Identity;

public sealed class PasswordResetService : IPasswordResetService
{
    private readonly IUserService _userService;
    private readonly IAuthenticationLinkService _linkService;
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly AuthenticationEmailOptions _emailOptions;

    public PasswordResetService(
        IUserService userService,
        IAuthenticationLinkService linkService,
        IEmailService emailService,
        IEmailTemplateService emailTemplateService,
        IOptions<AuthenticationEmailOptions> emailOptions)
    {
        _userService = userService;
        _linkService = linkService;
        _emailService = emailService;
        _emailTemplateService = emailTemplateService;
        _emailOptions = emailOptions.Value;
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
            // Deliberately don't expose whether the user exists.
            return Result<PasswordResetResult?>.Success(null);
        }

        var tokenResult =
            await _userService.GeneratePasswordResetTokenAsync(
                userResult.Value.UserId,
                cancellationToken);

        if (tokenResult.IsFailure)
        {
            return Result<PasswordResetResult?>.Success(null);
        }

        var resetLink =
            _linkService.CreatePasswordResetLink(
                userResult.Value.UserId,
                tokenResult.Value);

        var result = new PasswordResetResult(
            userResult.Value.UserId,
            userResult.Value.Email,
            userResult.Value.FirstName,
            resetLink);

        if (_emailOptions.Mode ==
            AuthenticationEmailMode.AuthenticationService)
        {
            var body =
                _emailTemplateService.RenderPasswordReset(
                    result.FirstName,
                    result.PasswordResetLink);

            var emailMessage = new EmailMessage(
                result.Email,
                "Reset your password",
                body);

            await _emailService.SendAsync(
                emailMessage,
                cancellationToken);

            // We own email delivery, so don't expose the reset link.
            return Result<PasswordResetResult?>.Success(null);
        }

        // Application owns email delivery.
        return Result<PasswordResetResult?>.Success(result);
    }
}