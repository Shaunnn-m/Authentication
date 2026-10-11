using FluentValidation;

namespace Authentication.Application.Features.Application.LoginApplicationUser;

public sealed class LoginApplicationUserCommandValidator
    : AbstractValidator<LoginApplicationUserCommand>
{
    public LoginApplicationUserCommandValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty()
            .WithMessage("Application is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
