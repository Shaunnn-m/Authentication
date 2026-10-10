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

        public GetApplicationSettingsQueryHandler(
            IApplicationSettingsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<GetApplicationSettingsResponse>> Handle(
            GetApplicationSettingsQuery request,
            CancellationToken cancellationToken)
        {
            var settings = await _repository.GetByApplicationIdAsync(
                request.ApplicationId,
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
