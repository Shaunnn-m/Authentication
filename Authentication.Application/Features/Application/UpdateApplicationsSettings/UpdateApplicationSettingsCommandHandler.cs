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
        private readonly ICurrentApplicationService _currentApplicationService;

        public UpdateApplicationSettingsCommandHandler(
            IApplicationSettingsRepository repository,
            ICurrentApplicationService currentApplicationService)
        {
            _repository = repository;
            _currentApplicationService = currentApplicationService;
        }

        public async Task<Result<UpdateApplicationSettingsResponse>> Handle(
            UpdateApplicationSettingsCommand request,
            CancellationToken cancellationToken)
        {
            var applicationId = _currentApplicationService.ApplicationId;

            if (!applicationId.HasValue || applicationId.Value == Guid.Empty)
            {
                return Result<UpdateApplicationSettingsResponse>.Failure(
                    ApplicationMessages.ContextRequired);
            }

            var settings = await _repository.GetByApplicationIdAsync(
                applicationId.Value,
                cancellationToken);

            if (settings is null)
            {
                return Result<UpdateApplicationSettingsResponse>.Failure(
                    ApplicationMessages.SettingsNotFound);
            }

            settings.Update(
                request.settings.EmailHandlingMode,
                request.settings.EnableEmailVerification);

            return Result<UpdateApplicationSettingsResponse>.Success(
                new UpdateApplicationSettingsResponse(
                    "Application settings updated successfully."));
        }
    }
}
