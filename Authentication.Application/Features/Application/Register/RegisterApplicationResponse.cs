namespace Authentication.Application.Features.Application.Register;

public sealed record RegisterApplicationResponse(
    Guid ApplicationId,
    string Name,
    string ClientId,
    string? ClientSecret);
