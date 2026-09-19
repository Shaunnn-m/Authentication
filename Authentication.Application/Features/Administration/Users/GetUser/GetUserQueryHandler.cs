using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Identity;
using MediatR;

namespace Authentication.Application.Features.Administration.Users.GetUser;

public sealed class GetUserQueryHandler
    : IRequestHandler<GetUserQuery, Result<GetUserResponse>>
{
    private readonly IAdminUserService _adminUserService;

    public GetUserQueryHandler(IAdminUserService adminUserService)
    {
        _adminUserService = adminUserService;
    }

    public async Task<Result<GetUserResponse>> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var userResult = await _adminUserService.GetUserAsync(
            request.IdentifierType,
            request.Identifier,
            cancellationToken);
        
        if (userResult.IsFailure)
        {
            return Result<GetUserResponse>.Failure(userResult.Error!);
        }

        var user = userResult.Value!;

        return Result<GetUserResponse>.Success(
            new GetUserResponse(
                user.UserId,
                user.FirstName,
                user.LastName,
                user.Email,
                user.IsActive,
                user.EmailConfirmed,
                user.Roles));

    }
}
