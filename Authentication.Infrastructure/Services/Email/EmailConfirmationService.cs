
using Authentication.Application.Abstractions.Email;
using Authentication.Application.Common.Authentication;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Interfaces.Identity;
using Microsoft.Extensions.Options;

namespace Authentication.Infrastructure.Services.Email
{
    public sealed class EmailConfirmationService : IEmailConfirmationService
    {
        private readonly IUserService _userService;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IConfirmationLinkService _confirmationLinkService;
        private readonly IEmailService _emailService;
        private readonly EmailConfirmationOptions _options;

        public EmailConfirmationService(
            IUserService userService,
            IConfirmationLinkService confirmationLinkService,
            IEmailService emailService,
            IEmailTemplateService emailTemplateService,
            IOptions<EmailConfirmationOptions> options)
        {
            _userService = userService;
            _confirmationLinkService = confirmationLinkService;
            _emailService = emailService;
            _emailTemplateService = emailTemplateService;
            _options = options.Value;
        }

        public async Task<Result<EmailConfirmationResult?>> HandleAsync(
            Guid userId,
            string firstName,
            string email,
            CancellationToken cancellationToken = default)
        {
            if (!_options.Enabled)
            {
                return Result<EmailConfirmationResult?>.Success(null);
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
                _confirmationLinkService.CreateConfirmationLink(
                    userId,
                    tokenResult.Value);

            var confirmationResult = new EmailConfirmationResult(
                userId,
                email,
                tokenResult.Value,
                confirmationLink);

            if (_options.Mode == EmailConfirmationMode.AuthenticationService)
            {
                var body = _emailTemplateService
               .RenderRegistrationConfirmation(
                   firstName,
                   confirmationLink);

                var emailMessage = new EmailMessage(
                    email,
                    "Confirm your account",
                    body);

                await _emailService.SendAsync(
                    emailMessage,
                    cancellationToken);
            }

            return Result<EmailConfirmationResult?>.Success(
                confirmationResult);
        }
    }
}
