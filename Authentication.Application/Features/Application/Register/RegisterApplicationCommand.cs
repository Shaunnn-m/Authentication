using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using Authentication.Domain.Enums;
using MediatR;

namespace Authentication.Application.Features.Application.Register;

public sealed record RegisterApplicationCommand(
    string Name,
    ApplicationClientType ClientType,
    RegisterApplicationSettingsRequest? Settings = null)
    : IRequest<Result<RegisterApplicationResponse>>, IUnitOfWorkCommand;

public sealed record RegisterApplicationSettingsRequest
{
    public bool? EnableEmailVerification { get; init; }
    public EmailHandlingMode? EmailHandlingMode { get; init; }
}