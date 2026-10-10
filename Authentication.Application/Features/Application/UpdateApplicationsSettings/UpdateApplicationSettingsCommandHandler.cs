using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Features.Applications.UpdateApplicationSettings;
using Authentication.Application.Interfaces.Application;
using MediatR;

namespace Authentication.Application.Features.Application.UpdateApplicationsSettings
{
    public sealed class UpdateApplicationSettingsCommandHandler
    : IRequestHandler<
        UpdateApplicationSettingsCommand,
        Result<UpdateApplicationSettingsResponse>>
    {
        private readonly IApplicationSettingsRepository _repository;

        public UpdateApplicationSettingsCommandHandler(
            IApplicationSettingsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<UpdateApplicationSettingsResponse>> Handle(
            UpdateApplicationSettingsCommand request,
            CancellationToken cancellationToken)
        {
            var settings = await _repository.GetByApplicationIdAsync(
                request.ApplicationId,
                cancellationToken);

            if (settings is null)
            {
                return Result<UpdateApplicationSettingsResponse>.Failure(
                    ApplicationMessages.SettingsNotFound);
            }

            settings.Update(
                request.EmailVerificationEnabled,
                request.PasswordResetEnabled,
                request.AllowRegistration);

            return Result<UpdateApplicationSettingsResponse>.Success(
                new UpdateApplicationSettingsResponse(
                    "Application settings updated successfully."));
        }
    }
}
