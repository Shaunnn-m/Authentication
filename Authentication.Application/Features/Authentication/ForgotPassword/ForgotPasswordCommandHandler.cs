using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using MediatR;

namespace Authentication.Application.Features.Authentication.ForgotPassword;

public sealed class ForgotPasswordCommandHandler
    : IRequestHandler<
        ForgotPasswordCommand,
        Result<ForgotPasswordResponse>>
{
    private readonly IPasswordService _passwordService;

    public ForgotPasswordCommandHandler(
        IPasswordService passwordService)
    {
        _passwordService = passwordService;
    }

    public async Task<Result<ForgotPasswordResponse>> Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var result =
            await _passwordService.RequestAsync(
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