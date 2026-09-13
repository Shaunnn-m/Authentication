using Authentication.Application.Common.Results;
using Authentication.Application.Common.Messages;
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

    public async Task<Result<Guid>> CreateAsync(
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
            var errors = string.Join(
                ", ",
                result.Errors.Select(error => error.Description));

            return Result<Guid>.Failure(
                UserMessages.CreationFailed);
        }

        return Result<Guid>.Success(user.Id);
    }

    public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return Result<string>.Failure(
                UserMessages.NotFound);
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return Result<string>.Success(token);
    }

    public async Task<Result<string>> ConfirmEmailAsync(
    Guid userId,
    string token,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return Result<string>.Failure(
                UserMessages.NotFound);
        }

        var result = await _userManager.ConfirmEmailAsync(
            user,
            token);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(error => error.Description));

            return Result<string>.Failure(
                UserMessages.EmailConfirmationFailed with
                {
                    Message = errors
                });
        }

        return Result<string>.Success(
            "Email confirmed successfully.");
    }

    public async Task<Result<(Guid UserId, string FirstName, string Email)>>
    GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return Result<(Guid, string, string)>.Failure(
                UserMessages.NotFound);
        }

        return Result<(Guid, string, string)>.Success(
            (user.Id, user.FirstName, user.Email!));
    }

    public async Task<Result<bool>> IsEmailConfirmedAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return Result<bool>.Failure(
                UserMessages.NotFound);
        }

        return Result<bool>.Success(
            user.EmailConfirmed);
    }

    public async Task<Result<Guid>> ValidateCredentialsAsync(
    string email,
    string password,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return Result<Guid>.Failure(
                UserMessages.InvalidCredentials);
        }

        var passwordValid = await _userManager.CheckPasswordAsync(
            user,
            password);

        if (!passwordValid)
        {
            return Result<Guid>.Failure(
                UserMessages.InvalidCredentials);
        }

        return Result<Guid>.Success(user.Id);
    }

    public async Task<Result<bool>> IsActiveAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return Result<bool>.Failure(
                UserMessages.NotFound);
        }

        return Result<bool>.Success(user.IsActive);
    }

    public async Task<Result<IReadOnlyList<string>>> GetRolesAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(
            userId.ToString());

        if (user is null)
        {
            return Result<IReadOnlyList<string>>.Failure(
                UserMessages.NotFound);
        }

        var roles = await _userManager.GetRolesAsync(user);

        return Result<IReadOnlyList<string>>.Success(
            roles.ToList());
    }
}