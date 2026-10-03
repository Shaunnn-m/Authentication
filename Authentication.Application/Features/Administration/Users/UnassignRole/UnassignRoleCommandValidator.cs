using FluentValidation;

namespace Authentication.Application.Features.Administration.Users.UnassignRole;

public sealed class UnassignRoleCommandValidator
    : AbstractValidator<UnassignRoleCommand>
{
    public UnassignRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Role)
            .IsInEnum();
    }
}