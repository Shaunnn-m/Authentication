using Authentication.Application.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;

namespace Authentication.Infrastructure.Services.Identity;

public sealed class ConfirmationLinkService : IConfirmationLinkService
{
    private readonly IConfiguration _configuration;

    public ConfirmationLinkService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateConfirmationLink(
        Guid userId,
        string token)
    {
        var baseUrl = _configuration["Authentication:ConfirmationBaseUrl"]
            ?? throw new InvalidOperationException(
                "Confirmation base URL is not configured.");

        var encodedToken = Uri.EscapeDataString(token);

        return $"{baseUrl}?userId={userId}&token={encodedToken}";
    }
}