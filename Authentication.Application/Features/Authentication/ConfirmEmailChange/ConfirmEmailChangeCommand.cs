using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Authentication.ConfirmEmailChange;

public sealed record ConfirmEmailChangeCommand(
    Guid UserId,
    string NewEmail,
    string Token
) : IRequest<Result<ConfirmEmailChangeResponse>>, IUnitOfWorkCommand;
