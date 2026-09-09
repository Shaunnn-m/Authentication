namespace Authentication.Application.Interfaces.Authentication;

public interface ITokenService
{
    string GenerateAccessToken(
        Guid userId,
        string email,
        IEnumerable<string> roles);
}