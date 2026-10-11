
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using Authentication.Domain.Enums;
using MediatR;

namespace Authentication.Application.Features.Applications.UpdateApplicationSettings;

public sealed record UpdateApplicationSettingsCommand(
    UpdateApplicationSettingsRequest settings)
    : IRequest<Result<UpdateApplicationSettingsResponse>>, IUnitOfWorkCommand;

public sealed record UpdateApplicationSettingsRequest
{
    public EmailHandlingMode EmailHandlingMode { get; init; }
    public bool EnableEmailVerification { get; init; }
}