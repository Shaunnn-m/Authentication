using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Authorization.AssignRole;

public sealed class AssignRoleCommandHandler
    : IRequestHandler<AssignRoleCommand, Result<AssignRoleResponse>>
{
    private readonly IUserService _userService;

    public AssignRoleCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<AssignRoleResponse>> Handle(
        AssignRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.AddToRoleAsync(
            request.UserId,
            request.Role,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<AssignRoleResponse>.Failure(
                result.Error!);
        }

        return Result<AssignRoleResponse>.Success(
            new AssignRoleResponse(
                request.UserId,
                request.Role,
                "Role assigned successfully."));
        
    }
}
