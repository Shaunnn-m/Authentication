using Authentication.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Infrastructure.Identity;

public sealed class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> ExistsByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user is not null;
    }

    public async Task<(bool Succeeded, Guid UserId, IEnumerable<string> Errors)> CreateAsync(
        string firstName,
        string lastName,
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        var result = await _userManager.CreateAsync(
            user,
            password);

        if (!result.Succeeded)
        {
            return (
                false,
                Guid.Empty,
                result.Errors.Select(error => error.Description));
        }

        return (
            true,
            user.Id,
            Enumerable.Empty<string>());
    }
}