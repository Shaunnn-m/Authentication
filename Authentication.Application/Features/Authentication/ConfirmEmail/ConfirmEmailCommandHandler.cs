
using Authentication.Application.Common.Results;
using Authentication.Application.Features.Authentication.ConfirmEmail;
using Authentication.Application.Interfaces.Identity;
using MediatR;

public sealed class ConfirmEmailCommandHandler
    : IRequestHandler<
        ConfirmEmailCommand,
        Result<ConfirmEmailResponse>>
{
    private readonly IUserService _userService;

    public ConfirmEmailCommandHandler(
        IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<ConfirmEmailResponse>> Handle(
        ConfirmEmailCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.ConfirmEmailAsync(
            request.UserId,
            request.Token,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<ConfirmEmailResponse>.Failure(
                result.Error!);
        }

        return Result<ConfirmEmailResponse>.Success(
            new ConfirmEmailResponse(
                request.UserId,
                string.Empty,
                result.Value));
    }
}