using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Abstractions.Identity;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Interfaces.Identity;
using Authentication.Domain.Applications;
using Authentication.Domain.Enums;
using MediatR;

namespace Authentication.Application.Features.Application.RegisterApplicationUser;

public sealed class RegisterApplicationUserCommandHandler
    : IRequestHandler<
        RegisterApplicationUserCommand,
        Result<RegisterApplicationUserResponse>>
{
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IEmailService _emailService;
    private readonly IAdminUserService _adminUserService;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationUserAccessRepository _applicationUserAccessRepository;
    private readonly IApplicationSettingsRepository _applicationSettingsRepository;

    public RegisterApplicationUserCommandHandler(
        IUserService userService,
        IPasswordService passwordService,
        IEmailService emailService,
        IAdminUserService adminUserService,
        IApplicationRepository applicationRepository,
        IApplicationUserAccessRepository applicationUserAccessRepository,
        IApplicationSettingsRepository applicationSettingsRepository)
    {
        _userService = userService;
        _passwordService = passwordService;
        _emailService = emailService;
        _adminUserService = adminUserService;
        _applicationRepository = applicationRepository;
        _applicationUserAccessRepository = applicationUserAccessRepository;
        _applicationSettingsRepository = applicationSettingsRepository;
    }

    public async Task<Result<RegisterApplicationUserResponse>> Handle(
        RegisterApplicationUserCommand request,
        CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetByAppplicationId(
            request.ApplicationId,
            cancellationToken);

        if (application is null)
        {
            return Result<RegisterApplicationUserResponse>.Failure(
                ApplicationMessages.NotFound);
        }

        if (!application.IsActive)
        {
            return Result<RegisterApplicationUserResponse>.Failure(
                ApplicationMessages.Inactive);
        }

        var settings = await _applicationSettingsRepository.GetByApplicationIdAsync(
            application.Id,
            cancellationToken);

        if (settings is null)
        {
            return Result<RegisterApplicationUserResponse>.Failure(
                ApplicationMessages.SettingsNotFound);
        }

        var userExists = await _userService.ExistsByEmailAsync(
            request.Email,
            cancellationToken);

        Guid userId;
        if (userExists)
        {
            var credentialsResult = await _passwordService.ValidateCredentialsAsync(
                request.Email,
                request.Password,
                cancellationToken);

            if (credentialsResult.IsFailure)
            {
                var existingUserResult = await _userService.GetByEmailAsync(
                    request.Email,
                    cancellationToken);

                if (existingUserResult.IsFailure)
                {
                    return Result<RegisterApplicationUserResponse>.Failure(
                        existingUserResult.Error!);
                }

                var existingApplicationNames =
                    await _applicationUserAccessRepository
                        .GetActiveApplicationNamesByUserIdAsync(
                            existingUserResult.Value.UserId,
                            cancellationToken);

                return Result<RegisterApplicationUserResponse>.Failure(
                    UserMessages.ExistingAccountCredentialsRequired(
                        existingApplicationNames,
                        application.Name));
            }

            userId = credentialsResult.Value;

            var existingAccess = await _applicationUserAccessRepository.GetAsync(
                application.Id,
                userId,
                cancellationToken);

            if (existingAccess is not null)
            {
                if (!existingAccess.IsActive)
                {
                    existingAccess.Activate();
                }

                return Result<RegisterApplicationUserResponse>.Success(
                    new RegisterApplicationUserResponse(
                        userId,
                        request.Email));
            }
        }
        else
        {
            var isActive = settings.EmailHandlingMode !=
                EmailHandlingMode.PlatformManaged;

            var createResult = await _userService.CreateAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                isActive,
                cancellationToken);

            if (createResult.IsFailure)
            {
                return Result<RegisterApplicationUserResponse>.Failure(
                    createResult.Error!);
            }

            userId = createResult.Value;

            var roleResult = await _adminUserService.AddToRoleAsync(
                userId,
                UserRole.Customer,
                cancellationToken);

            if (roleResult.IsFailure)
            {
                return Result<RegisterApplicationUserResponse>.Failure(
                    roleResult.Error!);
            }

            if (settings.EmailHandlingMode == EmailHandlingMode.PlatformManaged)
            {
                var confirmationResult = await _emailService.HandleAsync(
                    userId,
                    request.FirstName,
                    request.Email,
                    application.Id,
                    cancellationToken);

                if (confirmationResult.IsFailure)
                {
                    return Result<RegisterApplicationUserResponse>.Failure(
                        confirmationResult.Error!);
                }
            }
        }

        await _applicationUserAccessRepository.AddAsync(
            ApplicationUserAccess.Create(application.Id, userId),
            cancellationToken);

        return Result<RegisterApplicationUserResponse>.Success(
            new RegisterApplicationUserResponse(
                userId,
                request.Email));
    }
}
