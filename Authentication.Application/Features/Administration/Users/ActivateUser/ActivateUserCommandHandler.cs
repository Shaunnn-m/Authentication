using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.ActivateUser;

public sealed class ActivateUserCommandHandler
    : IRequestHandler<ActivateUserCommand, Result<ActivateUserResponse>>
{
    private readonly IUserService _userService;

    public ActivateUserCommandHandler(
        IUserService userService)
    {
        _userService = userService;
    }
    public async Task<Result<ActivateUserResponse>> Handle(
        ActivateUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.ActivateAsync(
            request.UserId,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<ActivateUserResponse>.Failure(
                    result.Error!);
        }

        return Result<ActivateUserResponse>.Success(
            new ActivateUserResponse(
                request.UserId,
                "The user account has been activated."));
    }
}
