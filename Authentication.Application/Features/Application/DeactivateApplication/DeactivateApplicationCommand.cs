using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Application.DeactivateApplication;

public sealed record DeactivateApplicationCommand(Guid ApplicationId)
    : IRequest<Result<Guid>>, IUnitOfWorkCommand;
