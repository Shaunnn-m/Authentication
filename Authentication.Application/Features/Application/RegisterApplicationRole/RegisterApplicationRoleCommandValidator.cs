using FluentValidation;

namespace Authentication.Application.Features.Application.RegisterApplicationRole
{
    public sealed class RegisterApplicationRoleCommandValidator
    : AbstractValidator<RegisterApplicationRoleCommand>
    {
        public RegisterApplicationRoleCommandValidator()
        {
            RuleFor(x => x.ApplicationId)
                .NotEmpty()
                .WithMessage("Application ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Role name is required.")
                .MaximumLength(100)
                .WithMessage("Role name must not exceed 100 characters.");
        }
    }
}
