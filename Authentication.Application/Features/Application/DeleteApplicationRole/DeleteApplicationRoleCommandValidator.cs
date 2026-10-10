using FluentValidation;

namespace Authentication.Application.Features.Application.DeleteApplicationRole;

public sealed class DeleteApplicationRoleCommandValidator
    : AbstractValidator<DeleteApplicationRoleCommand>
{
    public DeleteApplicationRoleCommandValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty();

        RuleFor(x => x.RoleId)
            .NotEmpty();
    }
}
