using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.ForgotPassword;

public sealed record ForgotPasswordCommand(
    string Email) : IRequest<Result<ForgotPasswordResponse>>;
