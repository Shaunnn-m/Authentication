using FluentValidation;

namespace Authentication.Application.Features.Application.UpdateApplicationRole;

public sealed class UpdateApplicationRoleCommandValidator
    : AbstractValidator<UpdateApplicationRoleCommand>
{
    public UpdateApplicationRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
