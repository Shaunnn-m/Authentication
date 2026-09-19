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
    public async Task<Result<DeactivateAccountResponse>> Handle(
        DeactivateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (!userId.HasValue)
        {
            return Result<DeactivateAccountResponse>.Failure(
                UserMessages.NotFound);
        }

        var result = await _userService.DeactivateAccountAsync(
            userId.Value,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<DeactivateAccountResponse>.Failure(
                result.Error!);
        }

        return Result<DeactivateAccountResponse>.Success(
            new DeactivateAccountResponse(
                "Your account has been deactivated successfully."));
    }
}
