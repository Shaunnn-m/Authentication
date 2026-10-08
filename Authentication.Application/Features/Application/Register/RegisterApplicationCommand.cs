using Authentication.Application.Common.Results;
using Authentication.Application.Common.Results.Applications;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Application.Register;

public sealed record RegisterApplicationCommand(
    string Name)
    : IRequest<Result<RegisterApplicationResult>>, IUnitOfWorkCommand;