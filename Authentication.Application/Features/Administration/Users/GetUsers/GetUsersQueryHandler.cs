using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.GetUsers;

public sealed class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, Result<GetUsersResponse>>
{
    private readonly IAdminUserService _adminUserService;

    public GetUsersQueryHandler(IAdminUserService adminUserService)
    {
        _adminUserService = adminUserService;
    }

    public async Task<Result<GetUsersResponse>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _adminUserService.GetUsersAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<GetUsersResponse>.Failure(
                result.Error!);
        }

        var users = result.Value!;

        var responseUsers = users.Users
            .Select(user => new GetUserItemResponse(
                user.UserId,
                user.FirstName,
                user.LastName,
                user.Email,
                user.IsActive,
                user.EmailConfirmed,
                user.Roles))
            .ToList();

        return Result<GetUsersResponse>.Success(
            new GetUsersResponse(
                responseUsers,
                users.Page,
                users.PageSize,
                users.TotalCount,
                users.TotalPages));
    }
}
