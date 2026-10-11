using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Application.ForgotPassword;

public sealed record ApplicationForgotPasswordCommand(
    Guid ApplicationId,
    string Email)
    : IRequest<Result<ApplicationForgotPasswordResponse>>;
