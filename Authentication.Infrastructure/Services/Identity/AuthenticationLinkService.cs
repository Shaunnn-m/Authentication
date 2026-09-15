using Authentication.Application.Common.Authentication;
using Authentication.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Authentication.Infrastructure.Services.Identity;

public sealed class AuthenticationLinkService
    : IAuthenticationLinkService
{
    private readonly IConfiguration _configuration;

    public AuthenticationLinkService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateEmailConfirmationLink(
        Guid userId,
        string token)
    {
        var baseUrl = _configuration[
            "Authentication:ConfirmationBaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException(
                "Authentication confirmation base URL is not configured.");
        }

        return QueryHelpers.AddQueryString(
            baseUrl,
            new Dictionary<string, string?>
            {
                ["userId"] = userId.ToString(),
                ["token"] = token
            });
    }

    public string CreatePasswordResetLink(
        Guid userId,
        string token)
    {
        var baseUrl = _configuration[
            "Authentication:PasswordResetBaseUrl"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException(
                "Authentication password reset base URL is not configured.");
        }

        return QueryHelpers.AddQueryString(
            baseUrl,
            new Dictionary<string, string?>
            {
                ["userId"] = userId.ToString(),
                ["token"] = token
            });
    }
}