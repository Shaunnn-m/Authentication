using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Common;
using MediatR;

namespace Authentication.Application.Features.Authentication.ConfirmEmail;
public sealed record ConfirmEmailCommand(
    Guid UserId,
    string Token)
    : IRequest<Result<ConfirmEmailResponse>>, IUnitOfWorkCommand;