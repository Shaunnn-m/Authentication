using Authentication.Application.Common.Authentication;

namespace Authentication.Application.Interfaces.Authentication;

public interface ITokenService
{
    AccessTokenResult  GenerateAccessToken(
        Guid userId,
        string email,
        IEnumerable<string> roles);
}