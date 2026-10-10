
using Authentication.Application.Abstractions.Email;
using Authentication.Application.Abstractions.Results.Email;
using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Interfaces.Identity;
using Authentication.Domain.Applications;
using Authentication.Domain.Enums;
using Authentication.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Authentication.Infrastructure.Services.Email
{
    public sealed class EmailService : IEmailService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        private readonly IApplicationSettingsRepository _applicationSettingsRepository;

        private readonly SmtpEmailService _smtpEmailService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IAuthenticationLinkService _authenticationLinkService;

        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            IAuthenticationLinkService authenticationLinkService,
            IEmailTemplateService emailTemplateService,
            IRefreshTokenService refreshTokenService,
            SmtpEmailService smtpEmailService,
            IApplicationSettingsRepository applicationSettingsRepository,
            ILogger<EmailService> logger)
        {
            _userManager = userManager;
            _userService = userService;
            _authenticationLinkService = authenticationLinkService;
            _emailTemplateService = emailTemplateService;
            _refreshTokenService = refreshTokenService;
            _smtpEmailService = smtpEmailService;
            _applicationSettingsRepository = applicationSettingsRepository;
            _logger = logger;
        }

        public async Task SendAsync(
            EmailMessage email,
            CancellationToken cancellationToken = default)
        {
            await _smtpEmailService.SendAsync(
            email,
            cancellationToken);
        }

        public async Task<Result<EmailConfirmationResult?>> HandleAsync(
         Guid userId,
         string firstName,
         string email,
         Guid? applicationId = null,
         CancellationToken cancellationToken = default)
        {
            ApplicationSettings? applicationSettings = null;

            if (applicationId.HasValue)
            {
                applicationSettings =
                    await _applicationSettingsRepository.GetByApplicationIdAsync(
                        applicationId.Value,
                        cancellationToken);

                if (applicationSettings is null)
                {
                    return Result<EmailConfirmationResult?>.Failure(
                        ApplicationMessages.SettingsNotFound);
                }
            }

            var tokenResult =
                await _userService.GenerateEmailConfirmationTokenAsync(
                    userId,
                    cancellationToken);

            if (tokenResult.IsFailure)
            {
                return Result<EmailConfirmationResult?>.Failure(
                    tokenResult.Error!);
            }

            var confirmationLink =
                _authenticationLinkService.CreateEmailConfirmationLink(
                    userId,
                    tokenResult.Value!);

            var confirmationResult = new EmailConfirmationResult(
                userId,
                email,
                tokenResult.Value!,
                confirmationLink);

            // No ApplicationId means platform-managed email.
            var platformManaged =
                applicationSettings is null ||
                applicationSettings.EmailHandlingMode ==
                    EmailHandlingMode.PlatformManaged;

            if (platformManaged)
            {
                var body = _emailTemplateService.RenderRegistrationConfirmation(
                    firstName,
                    confirmationLink);

                var emailMessage = new EmailMessage(
                    email,
                    "Confirm your account",
                    body);

                await SendAsync(emailMessage, cancellationToken);
            }

            return Result<EmailConfirmationResult?>.Success(
                confirmationResult);
        }

        public async Task<Result<EmailChangeResult?>> RequestChangeAsync(
        Guid userId,
        string newEmail,
        Guid? applicationId = null,
        CancellationToken cancellationToken = default)
        {
            ApplicationSettings? applicationSettings = null;

            if (applicationId.HasValue)
            {
                applicationSettings =
                    await _applicationSettingsRepository.GetByApplicationIdAsync(
                        applicationId.Value,
                        cancellationToken);

                if (applicationSettings is null)
                {
                    return Result<EmailChangeResult?>.Failure(
                        ApplicationMessages.SettingsNotFound);
                }
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return Result<EmailChangeResult?>.Failure(
                    UserMessages.NotFound);
            }

            newEmail = newEmail.Trim();

            if (string.Equals(
                user.Email,
                newEmail,
                StringComparison.OrdinalIgnoreCase))
            {
                return Result<EmailChangeResult?>.Failure(
                    UserMessages.EmailAlreadyInUse);
            }

            var existingUser = await _userManager.FindByEmailAsync(newEmail);

            if (existingUser is not null && existingUser.Id != user.Id)
            {
                return Result<EmailChangeResult?>.Failure(
                    UserMessages.EmailAlreadyInUse);
            }

            var token = await _userManager.GenerateChangeEmailTokenAsync(
                user,
                newEmail);

            var link = _authenticationLinkService.CreateEmailChangeLink(
                user.Id,
                newEmail,
                token);

            var emailChangeResult = new EmailChangeResult(
                user.Id,
                newEmail,
                link);

            // No ApplicationId means platform-managed email.
            var platformManaged =
                applicationSettings is null ||
                applicationSettings.EmailHandlingMode ==
                    EmailHandlingMode.PlatformManaged;

            if (platformManaged)
            {
                var body = _emailTemplateService.RenderEmailChange(
                    user.FirstName,
                    newEmail,
                    link);

                var emailMessage = new EmailMessage(
                    newEmail,
                    "Confirm your email change",
                    body);

                await SendAsync(emailMessage, cancellationToken);
            }

            return Result<EmailChangeResult?>.Success(emailChangeResult);
        }


        public async Task<Result<bool>> ConfirmChangeAsync(
        Guid userId,
        string newEmail,
        string token,
        CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(
                userId.ToString());

            if (user is null)
            {
                return Result<bool>.Failure(
                    UserMessages.NotFound);
            }

            var existingUser =
                await _userManager.FindByEmailAsync(newEmail);

            if (existingUser is not null &&
                existingUser.Id != user.Id)
            {
                return Result<bool>.Failure(
                    UserMessages.EmailAlreadyInUse);
            }

            var result = await _userManager.ChangeEmailAsync(
                user,
                newEmail,
                token);

            if (!result.Succeeded)
            {
                return Result<bool>.Failure(
                    UserMessages.EmailChangeFailed);
            }

            var refreshTokenResult =
                await _refreshTokenService.RevokeAllAsync(
                    userId,
                    cancellationToken);

            if (refreshTokenResult.IsFailure)
            {
                return Result<bool>.Failure(
                    refreshTokenResult.Error!);
            }

            return Result<bool>.Success(true);
        }
    }

}
