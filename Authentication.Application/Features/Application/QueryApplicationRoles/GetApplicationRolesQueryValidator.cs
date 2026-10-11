using FluentValidation;

namespace Authentication.Application.Features.Application.QueryApplicationRoles;

public sealed class GetApplicationRolesQueryValidator
    : AbstractValidator<GetApplicationRolesQuery>
{
    public GetApplicationRolesQueryValidator()
    {
    }
}
