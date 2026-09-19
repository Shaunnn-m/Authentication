using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authentication.GetMyAccount;

public sealed class GetMyAccountQueryHandler
    : IRequestHandler<GetMyAccountQuery, Result<GetMyAccountResponse>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserService _userService;

    public GetMyAccountQueryHandler(
        ICurrentUser currentUser,
        IUserService userService)
    {
        _currentUser = currentUser;
        _userService = userService;
    }

    public async Task<Result<GetMyAccountResponse>> Handle(
        GetMyAccountQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated ||
            !_currentUser.UserId.HasValue)
        {
            return Result<GetMyAccountResponse>.Failure(
                UserMessages.AuthenticationRequired);
        }

        var userResult = await _userService.GetAccountDetailsAsync(
            _currentUser.UserId.Value,
            cancellationToken);

        if (userResult.IsFailure)
        {
            return Result<GetMyAccountResponse>.Failure(
                userResult.Error!);
        }

        var user = userResult.Value;

        return Result<GetMyAccountResponse>.Success(
            new GetMyAccountResponse(user!.UserId,
                user.FirstName,
                user.LastName,
                user.Email,
                user.IsActive,
                user.EmailConfirmed,
                user.Roles));
    }
}
