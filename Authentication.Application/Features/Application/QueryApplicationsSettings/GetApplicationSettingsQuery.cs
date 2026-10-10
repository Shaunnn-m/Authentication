using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Application.QueryApplicationsSettings
{
    public sealed record GetApplicationSettingsQuery(Guid ApplicationId)
    : IRequest<Result<GetApplicationSettingsResponse>>;
}
