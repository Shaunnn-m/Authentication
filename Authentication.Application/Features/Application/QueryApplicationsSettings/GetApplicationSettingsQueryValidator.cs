using FluentValidation;

namespace Authentication.Application.Features.Application.QueryApplicationsSettings;

public sealed class GetApplicationSettingsQueryValidator
    : AbstractValidator<GetApplicationSettingsQuery>
{
    public GetApplicationSettingsQueryValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty();
    }
}
