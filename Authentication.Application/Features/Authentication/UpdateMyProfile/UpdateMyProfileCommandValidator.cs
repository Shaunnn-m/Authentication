using FluentValidation;

namespace Authentication.Application.Features.Authentication.UpdateMyProfile;

public sealed class UpdateMyProfileCommandValidator
    : AbstractValidator<UpdateMyProfileCommand>
{
    public UpdateMyProfileCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);
    }
}
