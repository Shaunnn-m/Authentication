using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.DeactivateUser;

public sealed class DeactivateUserCommandHandler
    : IRequestHandler<DeactivateUserCommand, Result<DeactivateUserResponse>>
{

    private readonly IUserService _userService;

    public DeactivateUserCommandHandler(
        IUserService userService)
    {
        _userService = userService;
    }    
    public async Task<Result<DeactivateUserResponse>> Handle(
        DeactivateUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.DeactivateAccountAsync(
            request.UserId,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<DeactivateUserResponse>.Failure(
                result.Error!);
        }

        return Result<DeactivateUserResponse>.Success(
            new DeactivateUserResponse(
                request.UserId,
                "User account deactivated successfully."));
    }
}
