using FluentValidation;

namespace Authentication.Application.Features.Authentication.ResendEmailConfirmation;

public sealed class ResendEmailConfirmationCommandValidator
    : AbstractValidator<ResendEmailConfirmationCommand>
{
    public ResendEmailConfirmationCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}
