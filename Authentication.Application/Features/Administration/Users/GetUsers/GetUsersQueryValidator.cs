using FluentValidation;

namespace Authentication.Application.Features.Administration.Users.GetUsers;

public sealed class GetUsersQueryValidator
    : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}
