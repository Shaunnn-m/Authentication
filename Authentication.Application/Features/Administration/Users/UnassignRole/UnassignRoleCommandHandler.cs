using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.UnassignRole;

public sealed class UnassignRoleCommandHandler :
    IRequestHandler<UnassignRoleCommand, Result<UnassignRoleResponse>>
{

    private readonly IAdminUserService _adminUserService;

    public UnassignRoleCommandHandler(IAdminUserService adminUserService)
    {
        _adminUserService = adminUserService;
    }
    
    public async Task<Result<UnassignRoleResponse>> Handle(
        UnassignRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _adminUserService.RemoveFromRoleAsync(
            request.UserId,
            request.Role.ToString(),
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<UnassignRoleResponse>.Failure(
                result.Error!);
        }

        return Result<UnassignRoleResponse>.Success(
            new UnassignRoleResponse(
                request.UserId,
                request.Role,
                "The role was removed from the user."));
    }
}