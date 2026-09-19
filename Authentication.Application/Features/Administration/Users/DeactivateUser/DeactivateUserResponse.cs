namespace Authentication.Application.Features.Administration.Users.DeactivateUser;

public sealed record DeactivateUserResponse(
    Guid UserId,
    string Message);
