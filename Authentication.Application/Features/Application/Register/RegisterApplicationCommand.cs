using Authentication.Application.Common.Results.Applications;
using MediatR;

namespace Authentication.Application.Features.Application.Register;

public sealed record RegisterApplicationCommand(
    string Name,
    Guid Application)
    : IRequest<RegisterApplicationResult>;