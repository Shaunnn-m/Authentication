using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authentication.ResetPassword;

public sealed class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordResponse>>
{
    private readonly IPasswordService _passwordService;

    public ResetPasswordCommandHandler(IPasswordService passwordService)
    {
        _passwordService = passwordService;
    }

    public async Task<Result<ResetPasswordResponse>> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var result =
            await _passwordService.ResetPasswordAsync(
                request.UserId,
                request.Token,
                request.Password,
                cancellationToken);

        if (result.IsFailure)
        {
            return Result<ResetPasswordResponse>.Failure(
                result.Error!);
        }

        return Result<ResetPasswordResponse>.Success(
            new ResetPasswordResponse(
                "Your password has been reset successfully."));
    }
}
