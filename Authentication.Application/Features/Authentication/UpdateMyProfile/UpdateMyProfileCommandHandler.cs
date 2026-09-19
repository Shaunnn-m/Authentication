using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authentication.UpdateMyProfile;

public sealed class UpdateMyProfileCommandHandler
    : IRequestHandler<UpdateMyProfileCommand, Result<UpdateMyProfileResponse>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IUserService _userService;

    public UpdateMyProfileCommandHandler(
        ICurrentUser currentUser,
        IUserService userService)
    {
        _currentUser = currentUser;
        _userService = userService;
    }

    public async Task<Result<UpdateMyProfileResponse>> Handle(
        UpdateMyProfileCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated ||
            !_currentUser.UserId.HasValue)
        {
            return Result<UpdateMyProfileResponse>.Failure(
                UserMessages.AuthenticationRequired);
        }

        var result = await _userService.UpdateProfileAsync(
            _currentUser.UserId.Value,
            request.FirstName,
            request.LastName,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<UpdateMyProfileResponse>.Failure(
                result.Error!);
        }

        var account = result.Value!;

        return Result<UpdateMyProfileResponse>.Success(
            new UpdateMyProfileResponse(
                account.FirstName,
                account.LastName,
                "Profile updated successfully."));
    }
}
