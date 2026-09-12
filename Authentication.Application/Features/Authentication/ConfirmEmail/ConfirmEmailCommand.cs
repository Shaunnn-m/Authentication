using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.ConfirmEmail;
public sealed record ConfirmEmailCommand(
    Guid UserId,
    string Token)
    : IRequest<Result<ConfirmEmailResponse>>;