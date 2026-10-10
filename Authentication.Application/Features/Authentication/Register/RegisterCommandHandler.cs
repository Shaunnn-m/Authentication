using Authentication.Application.Interfaces.Identity;
using Authentication.Application.Common.Results;
using MediatR;
using Authentication.Application.Common.Messages;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Abstractions.Identity;
using Authentication.Application.Interfaces.Applications;
using Authentication.Domain.Applications;
using Authentication.Application.Interfaces.Application;
using Authentication.Domain.Enums;

namespace Authentication.Application.Features.Authentication.Register;

public sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly IAdminUserService _adminUserService;
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationUserAccessRepository _applicationUserAccessRepository;
    private readonly IApplicationSettingsRepository _applicationSettingsRepository;

    public RegisterCommandHandler(IUserService userService, 
        IEmailService emailService,
        IAdminUserService adminUserService,
        IApplicationRepository applicationRepository,
        IApplicationUserAccessRepository applicationUserAccessRepository,
        IApplicationSettingsRepository applicationSettingsRepository)

    {
        _userService = userService;
        _emailService = emailService;
        _adminUserService = adminUserService;
        _applicationRepository = applicationRepository;
        _applicationUserAccessRepository = applicationUserAccessRepository;
        _applicationSettingsRepository = applicationSettingsRepository;
    }

    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetByAppplicationId(
        request.ApplicationId,
        cancellationToken);

        if (application is null)
        {
            return Result<RegisterResponse>.Failure(
                ApplicationMessages.NotFound);
        }

        var settings = await _applicationSettingsRepository.GetByApplicationIdAsync(
        application.Id,
        cancellationToken);

        if (!application.IsActive)
        {
            return Result<RegisterResponse>.Failure(
                ApplicationMessages.Inactive);
        }

        var exists = await _userService.ExistsByEmailAsync(
            request.Email,
            cancellationToken);

        if (exists)
        {
            return Result<RegisterResponse>.Failure(
                UserMessages.AlreadyExists);
        }

        var isActive = settings.EmailHandlingMode == EmailHandlingMode.PlatformManaged ? false : true;

        var result = await _userService.CreateAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            isActive,
            cancellationToken);

        if (result.IsFailure)
        {
            return Result<RegisterResponse>.Failure(
                UserMessages.AlreadyExists);
        }

        var roleResult = await _adminUserService.AddToRoleAsync(
            result.Value,
            UserRole.Customer,
            cancellationToken);

        if (roleResult.IsFailure)
        {
            return Result<RegisterResponse>.Failure(
                roleResult.Error!);
        }

        if (settings.EmailHandlingMode == EmailHandlingMode.PlatformManaged)
        {
            var confirmationResult = await _emailService.HandleAsync(
                result.Value,
                application.Id, 
                request.FirstName,
                request.Email,
                cancellationToken);

            if (confirmationResult.IsFailure)
            {
                return Result<RegisterResponse>.Failure(
                    confirmationResult.Error!);
            }
        }

        var applicationAccess = ApplicationUserAccess.Create(
            application.Id,
            result.Value);

        await _applicationUserAccessRepository.AddAsync(
            applicationAccess,
            cancellationToken);

        return Result<RegisterResponse>.Success(
            new RegisterResponse(
                result.Value,
                request.Email));
    }
}