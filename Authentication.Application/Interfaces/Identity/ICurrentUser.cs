using Authentication.Application.Common.Results;

namespace Authentication.Application.Interfaces.Identity;

public interface ICurrentUser
{
    Guid? UserId { get; }

    string? Email { get; }

    bool IsAuthenticated { get; }

    IReadOnlyList<string> Roles { get; }
}
