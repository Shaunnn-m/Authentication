using FluentValidation;

namespace Authentication.Application.Features.Authentication.ChangeEmail;

public sealed class ChangeEmailCommandValidator
    : AbstractValidator<ChangeEmailCommand>
{
    public ChangeEmailCommandValidator()
    {

        RuleFor(x => x.NewEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}
