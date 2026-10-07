using Authentication.Application.Features.Application.Register;
using FluentValidation;

namespace Authentication.Application.Features.Applications.Register;

public sealed class RegisterApplicationCommandValidator
    : AbstractValidator<RegisterApplicationCommand>
{
    public RegisterApplicationCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

    }
}
