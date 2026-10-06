
namespace Authentication.Application.Abstractions.Results.RefreshToken
{
    public sealed record RefreshTokenValidationResult(
     Guid UserId,
     Guid ApplicationId);
}
