namespace Authentication.Application.Features.Administration.Users.ActivateUser;

public sealed record ActivateUserResponse(
    Guid UserId,
    string Message);
