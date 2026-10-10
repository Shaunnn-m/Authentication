namespace Authentication.Application.Features.Application.RegisterApplicationUser;

public sealed record RegisterApplicationUserResponse(
    Guid UserId,
    string Email);
