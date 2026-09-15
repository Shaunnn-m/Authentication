using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using MediatR;

namespace Authentication.Application.Features.Authentication.ForgotPassword;

public sealed class ForgotPasswordCommandHandler
    : IRequestHandler<
        ForgotPasswordCommand,
        Result<ForgotPasswordResponse>>
{
    private readonly IPasswordResetService _passwordResetService;

    public ForgotPasswordCommandHandler(
        IPasswordResetService passwordResetService)
    {
        _passwordResetService = passwordResetService;
    }

    public async Task<Result<ForgotPasswordResponse>> Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var result =
            await _passwordResetService.RequestAsync(
                request.Email,
                cancellationToken);

        if (result.IsFailure)
        {
            return Result<ForgotPasswordResponse>.Failure(
                result.Error!);
        }

        return Result<ForgotPasswordResponse>.Success(
            new ForgotPasswordResponse(
                request.Email,
                result.Value?.PasswordResetLink,
                "If an account exists for this email, " +
                "a password reset link has been sent."));
    }
}