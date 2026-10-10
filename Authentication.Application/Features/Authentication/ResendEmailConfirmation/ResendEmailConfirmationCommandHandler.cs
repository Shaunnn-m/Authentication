using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Interfaces.Identity;
using Authentication.Domain.Enums;
using MediatR;

namespace Authentication.Application.Features.Authentication.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationCommandHandler
    : IRequestHandler<
        ResendEmailConfirmationCommand,
        Result<ResendEmailConfirmationResponse>>
{
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;
    private readonly IApplicationSettingsRepository _applicationSettingsRepository;

    public ResendEmailConfirmationCommandHandler(
        IEmailService emailService,
        IUserService userService,
        IApplicationSettingsRepository applicationSettingsRepository)
    {
        _emailService = emailService;
        _userService = userService;
        _applicationSettingsRepository = applicationSettingsRepository;
    }

    public async Task<Result<ResendEmailConfirmationResponse>> Handle(
        ResendEmailConfirmationCommand request,
        CancellationToken cancellationToken)
    {
        var settings = await _applicationSettingsRepository.GetByApplicationIdAsync(
            request.ApplicationId,
            cancellationToken); 

        if (settings == null) {
            return Result<ResendEmailConfirmationResponse>.Failure(
                ApplicationMessages.NotFound);
        }

        var userResult = await _userService.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (userResult.IsFailure)
        {
            return Result<ResendEmailConfirmationResponse>.Failure(
                userResult.Error!);
        }

        var user = userResult.Value;

        var confirmedResult =
            await _userService.IsEmailConfirmedAsync(
                user.UserId,
                cancellationToken);

        if (confirmedResult.IsFailure)
        {
            return Result<ResendEmailConfirmationResponse>.Failure(
                confirmedResult.Error!);
        }

        if (confirmedResult.Value)
        {
            return Result<ResendEmailConfirmationResponse>.Failure(
                UserMessages.EmailAlreadyConfirmed);
        }

        if (settings.EmailHandlingMode == EmailHandlingMode.PlatformManaged)
        {
            var confirmationResult =
                await _emailService.HandleAsync(
                    user.UserId,
                    user.FirstName,
                    user.Email,
                    request.ApplicationId,
                    cancellationToken);

            if (confirmationResult.IsFailure)
            {
                return Result<ResendEmailConfirmationResponse>.Failure(
                    confirmationResult.Error!);
            }
        }

        return Result<ResendEmailConfirmationResponse>.Success(
            new ResendEmailConfirmationResponse(
                request.Email,
                "Confirmation email sent successfully."));
    }
}
