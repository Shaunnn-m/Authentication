using Authentication.Application.Abstractions.Identity;
using Authentication.Application.Common.Authorization;
using FluentValidation;

namespace Authentication.Application.Features.Administration.Users.AssignRole;

public sealed class AssignRoleCommandValidator
    : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.");

        RuleFor(x => x.Role)
            .IsInEnum()
            .Must((command, role) =>
            {
                if (command.Role == UserRole.Admin || command.Role == UserRole.Customer)
                return true;
                return false;
            });
    }
}