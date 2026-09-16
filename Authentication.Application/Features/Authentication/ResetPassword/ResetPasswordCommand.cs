using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.ResetPassword;

public sealed record ResetPasswordCommand(
    Guid UserId,
    string Token,
    string Password,
    string ConfirmPassword) : IRequest<Result<ResetPasswordResponse>>;
