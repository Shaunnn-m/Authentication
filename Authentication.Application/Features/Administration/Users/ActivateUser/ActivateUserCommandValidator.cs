using FluentValidation;

namespace Authentication.Application.Features.Administration.Users.ActivateUser;

public sealed class ActivateUserCommandValidator
    : AbstractValidator<ActivateUserCommand>
{
    public ActivateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
