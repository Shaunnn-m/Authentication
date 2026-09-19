using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.ChangeEmail;

public sealed record ChangeEmailCommand(
    string NewEmail)
    : IRequest<Result<ChangeEmailResponse>>;
