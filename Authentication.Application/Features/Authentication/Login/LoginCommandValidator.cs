using FluentValidation;

namespace Authentication.Application.Features.Authentication.Login;

public sealed class LoginCommandValidator
    : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(x => x.ApplicationId)
            .NotEmpty()
            .WithMessage("Application is required.");

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
