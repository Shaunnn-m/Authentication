using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.AssignRole;

public sealed class AssignRoleCommandHandler
    : IRequestHandler<AssignRoleCommand, Result<AssignRoleResponse>>
{
    private readonly IAdminUserService _adminUserService;

    public AssignRoleCommandHandler(IAdminUserService adminUserService)
    {
        _adminUserService = adminUserService;
    }

    public async Task<Result<AssignRoleResponse>> Handle(
        AssignRoleCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _adminUserService.AddToRoleAsync(
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
