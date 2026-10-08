using Authentication.Application.Common.Results.Applications;
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

namespace Authentication.Application.Features.Applications.Register;

public sealed class RegisterApplicationCommandHandler
    : IRequestHandler<
        RegisterApplicationCommand,
        Result<RegisterApplicationResult>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationRoleRepository _roleRepository;
    private readonly ICurrentUser _currentUserService;
    private readonly IApplicationUserRoleRepository _userRoleRepository;
    private readonly IApplicationUserAccessRepository _accessRepository;

    public RegisterApplicationCommandHandler(
        IApplicationRepository applicationRepository,
        IApplicationRoleRepository roleRepository,
        ICurrentUser currentUserService,
        IApplicationUserRoleRepository userRoleRepository,
        IApplicationUserAccessRepository accessRepository)
    {
        _applicationRepository = applicationRepository;
        _roleRepository = roleRepository;
        _currentUserService = currentUserService;
        _userRoleRepository = userRoleRepository;
        _accessRepository = accessRepository;
    }

    public async Task<Result<RegisterApplicationResult>> Handle(
        RegisterApplicationCommand request,
        CancellationToken cancellationToken)
    {
         var userId = _currentUserService.UserId;

        if (userId is null)
        {
            return Result<RegisterApplicationResult>.Failure(
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

        return Result<RegisterApplicationResult>.Success(
            new RegisterApplicationResult(
                application.Id,
                application.Name,
                application.ClientId));
    }
}