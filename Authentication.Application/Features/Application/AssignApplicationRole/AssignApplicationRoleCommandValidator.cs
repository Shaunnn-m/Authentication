using Authentication.Application.Features.Applications.AssignApplicationRole;
using FluentValidation;

namespace Authentication.Application.Features.Applications.AssignApplicationRole;

public sealed class AssignApplicationRoleCommandValidator
    : AbstractValidator<AssignApplicationRoleCommand>
{
    public AssignApplicationRoleCommandValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.RoleId)
            .NotEmpty();
    }
}