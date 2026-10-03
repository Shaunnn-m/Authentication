using Authentication.Application.Interfaces.Identity;
using Authentication.Application.Common.Results;
using MediatR;
using Authentication.Application.Common.Messages;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Abstractions.Identity;
using Microsoft.Extensions.Logging;

namespace Authentication.Application.Features.Authentication.Register;

public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly IAdminUserService _adminUserService;

    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(IUserService userService, 
        IEmailService emailService,
        IAdminUserService adminUserService,
        ILogger<RegisterCommandHandler> logger)
    {
        _userService = userService;
        _emailService = emailService;
        _adminUserService = adminUserService;
        _logger = logger;
    }

    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
        "User registration requested for email {Email}.",
        request.Email);

        var exists = await _userService.ExistsByEmailAsync(
            request.Email,
            cancellationToken);

        if (exists)
        {
            _logger.LogWarning("User registration failed for email {Email}. User already exists.",request.Email);
            return Result<RegisterResponse>.Failure(
                UserMessages.AlreadyExists);
        }

        var result = await _userService.CreateAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("User registration failed for email {Email}.", request.Email);
            return Result<RegisterResponse>.Failure(
                UserMessages.AlreadyExists);
        }

        var roleResult = await _adminUserService.AddToRoleAsync(
            result.Value,
            UserRole.Customer,
            cancellationToken);

        if (roleResult.IsFailure)
        {
            _logger.LogWarning("User registration failed for email {Email}. Failed to add user to role.", request.Email);
            return Result<RegisterResponse>.Failure(
                roleResult.Error!);
        }

        var confirmationResult =
            await _emailService.HandleAsync(
                result.Value,
                request.FirstName,
                request.Email,
                cancellationToken);

        if (confirmationResult.IsFailure)
        {
            _logger.LogWarning("User registration failed for email {Email}. Failed to send confirmation email.", request.Email);
            return Result<RegisterResponse>.Failure(
                confirmationResult.Error!);
        }

        return Result<RegisterResponse>.Success(
            new RegisterResponse(
                result.Value,
                request.Email));
    }
}