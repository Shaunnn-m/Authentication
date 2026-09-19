using FluentValidation;

namespace Authentication.Application.Features.Authentication.ConfirmEmailChange;

public sealed class ConfirmEmailChangeCommandValidator
    : AbstractValidator<ConfirmEmailChangeCommand>
{
    public ConfirmEmailChangeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.NewEmail)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}
