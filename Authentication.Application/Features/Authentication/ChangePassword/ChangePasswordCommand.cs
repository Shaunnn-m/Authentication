using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.ChangePassword;

public sealed record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword,
    string ConfirmPassword)
    : IRequest<Result<ChangePasswordResponse>>;
