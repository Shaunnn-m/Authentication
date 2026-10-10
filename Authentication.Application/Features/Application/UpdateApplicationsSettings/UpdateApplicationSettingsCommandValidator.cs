using Authentication.Application.Features.Applications.UpdateApplicationSettings;
using FluentValidation;

namespace Authentication.Application.Features.Application.UpdateApplicationsSettings;

public sealed class UpdateApplicationSettingsCommandValidator
    : AbstractValidator<UpdateApplicationSettingsCommand>
{
    public UpdateApplicationSettingsCommandValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty();

    }
}
