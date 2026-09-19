using FluentValidation;

namespace Authentication.Application.Features.Administration.Users.GetUser;

public sealed class GetUserQueryValidator
    : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}