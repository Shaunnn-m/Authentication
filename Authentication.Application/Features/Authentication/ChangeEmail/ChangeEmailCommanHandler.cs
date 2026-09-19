
using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authentication.ChangeEmail;
public sealed class ChangeEmailCommandHandler
    : IRequestHandler<
        ChangeEmailCommand,
        Result<ChangeEmailResponse>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IEmailService _emailChangeService;

    public ChangeEmailCommandHandler(
        ICurrentUser currentUser,
        IEmailService emailChangeService)
    {
        _currentUser = currentUser;
        _emailChangeService = emailChangeService;
    }

    public async Task<Result<ChangeEmailResponse>> Handle(
        ChangeEmailCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.UserId.HasValue)
        {
            return Result<ChangeEmailResponse>.Failure(
                UserMessages.InvalidCredentials);
        }

        var result =
            await _emailChangeService.RequestChangeAsync(
                _currentUser.UserId.Value,
                request.NewEmail,
                cancellationToken);

        if (result.IsFailure)
        {
            return Result<ChangeEmailResponse>.Failure(
                result.Error!);
        }

        var value = result.Value!;

        return Result<ChangeEmailResponse>.Success(
            new ChangeEmailResponse(
                value.NewEmail,
                "A confirmation link has been sent to the new email address.",
                value.ConfirmationLink));
    }
}