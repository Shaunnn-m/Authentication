using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Email;
using MediatR;

namespace Authentication.Application.Features.Authentication.ConfirmEmailChange;
public sealed class ConfirmEmailChangeCommandHandler
    : IRequestHandler<
        ConfirmEmailChangeCommand,
        Result<ConfirmEmailChangeResponse>>
{
    private readonly IEmailService _emailChangeService;

    public ConfirmEmailChangeCommandHandler(
        IEmailService emailChangeService)
    {
        _emailChangeService = emailChangeService;
    }

    public async Task<Result<ConfirmEmailChangeResponse>> Handle(
        ConfirmEmailChangeCommand request,
        CancellationToken cancellationToken)
    {
        var result =
            await _emailChangeService.ConfirmChangeAsync(
                request.UserId,
                request.NewEmail,
                request.Token,
                cancellationToken);

        if (result.IsFailure)
        {
            return Result<ConfirmEmailChangeResponse>.Failure(
                result.Error!);
        }

        return Result<ConfirmEmailChangeResponse>.Success(
            new ConfirmEmailChangeResponse(
                request.NewEmail,
                "Email changed successfully. Please log in again."));
    }
}