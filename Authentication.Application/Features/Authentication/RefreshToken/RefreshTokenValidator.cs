using FluentValidation;

namespace Authentication.Application.Features.Authentication.RefreshToken;

public sealed class RefreshTokenValidator
    : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
