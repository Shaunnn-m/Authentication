using Authentication.Application.Abstractions.Identity;
using FluentValidation;

namespace Authentication.Application.Features.Administration.Users.GetUser;

public sealed class GetUserQueryValidator
    : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
         RuleFor(x => x.IdentifierType)
            .IsInEnum();

        RuleFor(x => x.Identifier)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Identifier)
            .Must((query, identifier) =>
            {
                if (query.IdentifierType == UserIdentifierType.Id)
                {
                    return Guid.TryParse(identifier, out _);
                }

                return true;
            })
            .WithMessage("The identifier is not a valid user ID.");
    }
}