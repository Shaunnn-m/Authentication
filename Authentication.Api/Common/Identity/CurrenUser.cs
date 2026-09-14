using System.Security.Claims;
using Authentication.Application.Interfaces.Identity;

namespace Authentication.Api.Common.Identity;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User =>
        _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public Guid? UserId
    {
        get
        {
            var userId = User?.FindFirstValue(
                ClaimTypes.NameIdentifier);

            return Guid.TryParse(userId, out var id)
                ? id
                : null;
        }
    }

    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email);

    public IReadOnlyList<string> Roles =>
        User?
            .FindAll(ClaimTypes.Role)
            .Select(claim => claim.Value)
            .ToList()
        ?? [];
}