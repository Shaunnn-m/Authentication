using Authentication.Application.Common.Authorization;
using FluentValidation;

namespace Authentication.Application.Features.Authorization.AssignRole;

public sealed class AssignRoleCommandValidator
    : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role =>
                role.Equals(
                    AppRoles.Admin,
                    StringComparison.OrdinalIgnoreCase)
                ||
                role.Equals(
                    AppRoles.Customer,
                    StringComparison.OrdinalIgnoreCase))
            .WithMessage("The specified role is invalid.");
    }
}