using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Application;
using MediatR;

namespace Authentication.Application.Features.Application.QueryApplicationsSettings
{
    public sealed class GetApplicationSettingsQueryHandler
     : IRequestHandler<
         GetApplicationSettingsQuery,
         Result<GetApplicationSettingsResponse>>
    {
        private readonly IApplicationSettingsRepository _repository;
        private readonly ICurrentApplicationService _currentApplicationService;

        public GetApplicationSettingsQueryHandler(
            IApplicationSettingsRepository repository,
            ICurrentApplicationService currentApplicationService)
        {
            _repository = repository;
            _currentApplicationService = currentApplicationService;
        }

        public async Task<Result<GetApplicationSettingsResponse>> Handle(
            GetApplicationSettingsQuery request,
            CancellationToken cancellationToken)
        {
            var applicationId = _currentApplicationService.ApplicationId;

            if (!applicationId.HasValue || applicationId.Value == Guid.Empty)
            {
                return Result<GetApplicationSettingsResponse>.Failure(
                    ApplicationMessages.ContextRequired);
            }

            var settings = await _repository.GetByApplicationIdAsync(
                applicationId.Value,
                cancellationToken);

            if (settings is null)
            {
                return Result<GetApplicationSettingsResponse>.Failure(
                    ApplicationMessages.SettingsNotFound);
            }

            return Result<GetApplicationSettingsResponse>.Success(
                new GetApplicationSettingsResponse(
                    settings.ApplicationId,
                    settings.EmailHandlingMode,
                    settings.IsEmailVerificationEnabled,
                    new DateTimeOffset(
                        DateTime.SpecifyKind(
                            settings.CreatedAt,
                            DateTimeKind.Utc)),
                    settings.UpdatedAt.HasValue
                        ? new DateTimeOffset(
                            DateTime.SpecifyKind(
                                settings.UpdatedAt.Value,
                                DateTimeKind.Utc))
                        : null));
        }
    }
}
