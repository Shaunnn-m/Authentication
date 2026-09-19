using FluentValidation;

namespace Authentication.Application.Features.Authentication.ReactivateAccount;

public sealed class ReactivateAccountCommandValidator
    : AbstractValidator<ReactivateAccountCommand>
{
    public ReactivateAccountCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);
    }
}