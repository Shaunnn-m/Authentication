using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authentication.DeactivateAccount;

public sealed class DeactivateAccountCommandHandler
    : IRequestHandler<DeactivateAccountCommand, Result<DeactivateAccountResponse>>
{
    private readonly IUserService _userService;
    private readonly ICurrentUser _currentUser;

    public DeactivateAccountCommandHandler(
        IUserService userService,
        ICurrentUser currentUser)
    {
        _userService = userService;
        _currentUser = currentUser;
    }
    public Task<Result<DeactivateAccountResponse>> Handle(
        DeactivateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (userId == null)
        {
            return Task.FromResult(
                Result<DeactivateAccountResponse>.Failure(
                    UserMessages.NotFound));
        }

        var result = _userService.DeactivateAccountAsync(
            userId.Value,
            cancellationToken);

        if (result.IsCompletedSuccessfully && result.Result.IsFailure)
        {
            return Task.FromResult(
                Result<DeactivateAccountResponse>.Failure(
                    result.Result.Error!));
        }

        return Task.FromResult(
            Result<DeactivateAccountResponse>.Success(
                new DeactivateAccountResponse(
                    "Your account has been deactivated successfully.")));
    }
}
