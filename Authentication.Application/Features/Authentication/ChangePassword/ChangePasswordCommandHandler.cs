using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authentication.ChangePassword;

public sealed class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand, Result<ChangePasswordResponse>>
{
    private readonly IPasswordService _passwordService;
    private readonly ICurrentUser _currentUser;

    public ChangePasswordCommandHandler(
        IPasswordService passwordService,
        ICurrentUser currentUser)
    {
        _passwordService = passwordService;
        _currentUser = currentUser;
    }

    public async Task<Result<ChangePasswordResponse>> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated ||
            !_currentUser.UserId.HasValue)
        {
            return Result<ChangePasswordResponse>.Failure(
                UserMessages.AuthenticationRequired);
        }
        
        var userId = _currentUser.UserId.Value;

        var result = await _passwordService.ChangePasswordAsync(
            userId,
            request.CurrentPassword,
            request.NewPassword,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<ChangePasswordResponse>.Failure(
                result.Error!);
        }

        return Result<ChangePasswordResponse>.Success(
            new ChangePasswordResponse(
                "Password changed successfully."));
    }
}
