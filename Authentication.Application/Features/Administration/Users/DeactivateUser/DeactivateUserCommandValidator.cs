using FluentValidation;

namespace Authentication.Application.Features.Administration.Users.DeactivateUser;

public sealed class DeactivateUserCommandValidator
    : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
