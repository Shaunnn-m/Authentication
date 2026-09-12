using FluentValidation;

namespace Authentication.Application.Features.Authentication.ConfirmEmail;

public sealed class ConfirmEmailCommandValidator
    : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Token)
            .NotEmpty();
    }
}