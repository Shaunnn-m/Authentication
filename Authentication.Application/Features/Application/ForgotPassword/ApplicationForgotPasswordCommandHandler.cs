using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Applications;
using Authentication.Application.Interfaces.Authentication;
using MediatR;

namespace Authentication.Application.Features.Application.ForgotPassword;

public sealed class ApplicationForgotPasswordCommandHandler
    : IRequestHandler<
        ApplicationForgotPasswordCommand,
        Result<ApplicationForgotPasswordResponse>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IPasswordService _passwordService;

    public ApplicationForgotPasswordCommandHandler(
        IApplicationRepository applicationRepository,
        IPasswordService passwordService)
    {
        _applicationRepository = applicationRepository;
        _passwordService = passwordService;
    }

    public async Task<Result<ApplicationForgotPasswordResponse>> Handle(
        ApplicationForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetByAppplicationId(
            request.ApplicationId,
            cancellationToken);

        if (application is null)
        {
            return Result<ApplicationForgotPasswordResponse>.Failure(
                ApplicationMessages.NotFound);
        }

        if (!application.IsActive)
        {
            return Result<ApplicationForgotPasswordResponse>.Failure(
                ApplicationMessages.Inactive);
        }

        var resetResult = await _passwordService.RequestAsync(
            request.Email,
            application.Id,
            cancellationToken);

        if (resetResult.IsFailure)
        {
            return Result<ApplicationForgotPasswordResponse>.Failure(
                resetResult.Error!);
        }

        return Result<ApplicationForgotPasswordResponse>.Success(
            new ApplicationForgotPasswordResponse(
                request.Email,
                resetResult.Value?.PasswordResetLink,
                "If an account exists for this email, a password reset link has been sent."));
    }
}
