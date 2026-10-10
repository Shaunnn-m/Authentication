using Authentication.Application.Common.Authorization;
using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
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
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationRoleRepository _applicationRoleRepository;
    private readonly IApplicationUserAccessRepository _applicationUserAccessRepository;
    private readonly IApplicationUserRoleRepository _applicationUserRoleRepository;
    private readonly IApplicationSettingsRepository _applicationSettingsRepository;

    public RegisterApplicationUserCommandHandler(
        IUserService userService,
        IPasswordService passwordService,
        IEmailService emailService,
        IApplicationRepository applicationRepository,
        IApplicationRoleRepository applicationRoleRepository,
        IApplicationUserAccessRepository applicationUserAccessRepository,
        IApplicationUserRoleRepository applicationUserRoleRepository,
        IApplicationSettingsRepository applicationSettingsRepository)
    {
        _userService = userService;
        _passwordService = passwordService;
        _emailService = emailService;
        _applicationRepository = applicationRepository;
        _applicationRoleRepository = applicationRoleRepository;
        _applicationUserAccessRepository = applicationUserAccessRepository;
        _applicationUserRoleRepository = applicationUserRoleRepository;
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

        var role = await _applicationRoleRepository.GetByNameAsync(
            application.Id,
            AppRoles.ApplicationCustomer,
            cancellationToken);

        if (role is null || role.ApplicationId != application.Id)
        {
            return Result<RegisterApplicationUserResponse>.Failure(
                ApplicationMessages.RoleNotFound);
        }

        if (!role.IsActive)
        {
            return Result<RegisterApplicationUserResponse>.Failure(
                ApplicationMessages.RoleInactive);
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
        }
        else
        {
            var isActive = !settings.IsEmailVerificationEnabled;

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

        var existingAccess = await _applicationUserAccessRepository.GetAsync(
            application.Id,
            userId,
            cancellationToken);

        if (existingAccess is null)
        {
            await _applicationUserAccessRepository.AddAsync(
                ApplicationUserAccess.Create(application.Id, userId),
                cancellationToken);
        }
        else if (!existingAccess.IsActive)
        {
            existingAccess.Activate();
        }

        var roleAlreadyAssigned =
            await _applicationUserRoleRepository.ExistsAsync(
                application.Id,
                userId,
                role.Id,
                cancellationToken);

        if (!roleAlreadyAssigned)
        {
            await _applicationUserRoleRepository.AddAsync(
                ApplicationUserRole.Create(
                    application.Id,
                    userId,
                    role.Id),
                cancellationToken);
        }

        return Result<RegisterApplicationUserResponse>.Success(
            new RegisterApplicationUserResponse(
                userId,
                request.Email));
    }
}
