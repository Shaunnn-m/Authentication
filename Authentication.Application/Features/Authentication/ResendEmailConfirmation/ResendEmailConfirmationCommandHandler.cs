using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authentication.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationCommandHandler
    : IRequestHandler<
        ResendEmailConfirmationCommand,
        Result<ResendEmailConfirmationResponse>>
{
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    public ResendEmailConfirmationCommandHandler(
        IEmailService emailService,
        IUserService userService)
    {
        _emailService = emailService;
        _userService = userService;
    }

    public async Task<Result<ResendEmailConfirmationResponse>> Handle(
        ResendEmailConfirmationCommand request,
        CancellationToken cancellationToken)
    {
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

        var confirmationResult =
            await _emailService.HandleAsync(
                user.UserId,
                user.FirstName,
                user.Email,
                cancellationToken);

        if (confirmationResult.IsFailure)
        {
            return Result<ResendEmailConfirmationResponse>.Failure(
                confirmationResult.Error!);
        }

        return Result<ResendEmailConfirmationResponse>.Success(
            new ResendEmailConfirmationResponse(
                request.Email,
                "Confirmation email sent successfully."));
    }
}
