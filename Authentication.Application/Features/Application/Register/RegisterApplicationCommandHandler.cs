using Authentication.Application.Features.Application.Register;
using Authentication.Application.Interfaces.Applications;
using ApplicationEntity = Authentication.Domain.Applications.Application;
using MediatR;
using Authentication.Application.Common.Results;
using Authentication.Application.Common.Authorization;
using Authentication.Application.Interfaces.Identity;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Common.Messages;
using Authentication.Domain.Applications;
using Authentication.Domain.Enums;

namespace Authentication.Application.Features.Applications.Register;

public sealed class RegisterApplicationCommandHandler
    : IRequestHandler<
        RegisterApplicationCommand,
        Result<RegisterApplicationResponse>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationRoleRepository _roleRepository;
    private readonly ICurrentUser _currentUserService;
    private readonly IApplicationUserRoleRepository _userRoleRepository;
    private readonly IApplicationUserAccessRepository _accessRepository;
    private readonly IApplicationSettingsRepository _applicationSettingsRepository;

    public RegisterApplicationCommandHandler(
        IApplicationRepository applicationRepository,
        IApplicationRoleRepository roleRepository,
        ICurrentUser currentUserService,
        IApplicationUserRoleRepository userRoleRepository,
        IApplicationUserAccessRepository accessRepository,
        IApplicationSettingsRepository applicationSettingsRepository)
    {
        _applicationRepository = applicationRepository;
        _roleRepository = roleRepository;
        _currentUserService = currentUserService;
        _userRoleRepository = userRoleRepository;
        _accessRepository = accessRepository;
        _applicationSettingsRepository = applicationSettingsRepository;
    }

    public async Task<Result<RegisterApplicationResponse>> Handle(
        RegisterApplicationCommand request,
        CancellationToken cancellationToken)
    {
         var userId = _currentUserService.UserId;

        if (userId is null)
        {
            return Result<RegisterApplicationResponse>.Failure(
                UserMessages.AuthenticationRequired);
        }

        var clientId = Guid.NewGuid().ToString("N");

        var application = ApplicationEntity.Create(
            request.Name.Trim(),
            clientId,
            userId.Value);

        await _applicationRepository.AddAsync(
            application,
            cancellationToken);

        var adminRole = ApplicationRole.Create(
            application.Id,
            AppRoles.Admin);

        await _roleRepository.AddAsync(
            adminRole,
            cancellationToken);

        var applicationAccess = ApplicationUserAccess.Create(
            application.Id,
            userId.Value);

        await _accessRepository.AddAsync(
            applicationAccess,
            cancellationToken);

        var userRole = ApplicationUserRole.Create(
            application.Id,
            userId.Value,
            adminRole.Id);

        await _userRoleRepository.AddAsync(
            userRole,
            cancellationToken);


        var requestedSettings = request.Settings;

        var applicationSettings = ApplicationSettings.Create(
            application.Id,
            requestedSettings?.EnableEmailVerification ?? true,
            requestedSettings?.EmailHandlingMode ?? EmailHandlingMode.PlatformManaged
            );

        await _applicationSettingsRepository.AddAsync(
            applicationSettings,
            cancellationToken);

        return Result<RegisterApplicationResponse>.Success(
            new RegisterApplicationResponse(
                application.Id,
                application.Name,
                application.ClientId));
    }
}