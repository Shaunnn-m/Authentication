using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.ChangePassword;

public sealed record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword,
    string ConfirmPassword)
    : IRequest<Result<ChangePasswordResponse>>;
