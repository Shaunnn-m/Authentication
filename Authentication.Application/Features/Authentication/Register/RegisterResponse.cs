namespace Authentication.Application.Features.Authentication.Register;

public sealed record RegisterResponse(
    Guid UserId,
    string Email);