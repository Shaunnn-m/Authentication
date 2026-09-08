namespace Authentication.Application.Common.Authentication;

public interface ITokenService
{
    string GenerateAccessToken(
        Guid userId,
        string email,
        IEnumerable<string> roles);
}