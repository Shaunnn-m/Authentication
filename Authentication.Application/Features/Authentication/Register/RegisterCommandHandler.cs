using Authentication.Application.Common.Results;
using MediatR;

namespace Authentication.Application.Features.Authentication.Register;

public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterCommandHandler(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(
            request.Email);

        if (existingUser is not null)
        {
            return Result<RegisterResponse>.Failure(
                "User.AlreadyExists",
                "A user with this email already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(
            user,
            request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(
                ", ",
                result.Errors.Select(error => error.Description));

            return Result<RegisterResponse>.Failure(
                "User.CreationFailed",
                errors);
        }

        return Result<RegisterResponse>.Success(
            new RegisterResponse(
                user.Id,
                user.Email!));
    }
}