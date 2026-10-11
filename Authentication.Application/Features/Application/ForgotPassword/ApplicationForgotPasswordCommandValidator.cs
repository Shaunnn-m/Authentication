using FluentValidation;

namespace Authentication.Application.Features.Application.ForgotPassword;

public sealed class ApplicationForgotPasswordCommandValidator
    : AbstractValidator<ApplicationForgotPasswordCommand>
{
    public ApplicationForgotPasswordCommandValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty()
            .WithMessage("Application is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}
