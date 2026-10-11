using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Features.Applications.AssignApplicationRole;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using Authentication.Application.Interfaces.Identity;
using Authentication.Domain.Applications;
using MediatR;

namespace Authentication.Application.Features.Applications.Roles.AssignApplicationRole;

public sealed class AssignApplicationRoleCommandHandler
    : IRequestHandler<
        AssignApplicationRoleCommand,
        Result<AssignApplicationRoleResponse>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationRoleRepository _roleRepository;
    private readonly IApplicationUserAccessRepository _accessRepository;
    private readonly IApplicationUserRoleRepository _userRoleRepository;
    private readonly IUserService _userService;
    private readonly ICurrentApplicationService _currentApplicationService;

    public AssignApplicationRoleCommandHandler(
        IApplicationRepository applicationRepository,
        IApplicationRoleRepository roleRepository,
        IApplicationUserAccessRepository accessRepository,
        IApplicationUserRoleRepository userRoleRepository,
        IUserService userService,
        ICurrentApplicationService currentApplicationService)
    {
        _applicationRepository = applicationRepository;
        _roleRepository = roleRepository;
        _accessRepository = accessRepository;
        _userRoleRepository = userRoleRepository;
        _userService = userService;
        _currentApplicationService = currentApplicationService;
    }

    public async Task<Result<AssignApplicationRoleResponse>> Handle(
        AssignApplicationRoleCommand request,
        CancellationToken cancellationToken)
    {
        var applicationId = _currentApplicationService.ApplicationId;

        if (!applicationId.HasValue || applicationId.Value == Guid.Empty)
        {
            return Result<AssignApplicationRoleResponse>.Failure(
                ApplicationMessages.ContextRequired);
        }

        var application = await _applicationRepository.GetByAppplicationId(
            applicationId.Value,
            cancellationToken);

        if (application is null)
        {
            return Result<AssignApplicationRoleResponse>.Failure(
                ApplicationMessages.NotFound);
        }

        if (!application.IsActive)
        {
            return Result<AssignApplicationRoleResponse>.Failure(
                ApplicationMessages.Inactive);
        }

        var role = await _roleRepository.GetByIdAsync(
            request.RoleId,
            cancellationToken);

        if (role is null ||
            role.ApplicationId != application.Id)
        {
            return Result<AssignApplicationRoleResponse>.Failure(
                ApplicationMessages.RoleNotFound);
        }

        if (!role.IsActive)
        {
            return Result<AssignApplicationRoleResponse>.Failure(
                ApplicationMessages.RoleInactive);
        }

        var userResult = await _userService.GetByIdAsync(
            request.UserId,
            cancellationToken);

        if (userResult.IsFailure)
        {
            return Result<AssignApplicationRoleResponse>.Failure(
                userResult.Error!);
        }

        var hasApplicationAccess =
            await _accessRepository.ExistsAsync(
                application.Id,
                request.UserId,
                cancellationToken);

        if (!hasApplicationAccess)
        {
            return Result<AssignApplicationRoleResponse>.Failure(
                ApplicationMessages.UserAccessRequired);
        }

        var alreadyAssigned =
            await _userRoleRepository.ExistsAsync(
                application.Id,
                request.UserId,
                request.RoleId,
                cancellationToken);

        if (alreadyAssigned)
        {
            return Result<AssignApplicationRoleResponse>.Failure(
                ApplicationMessages.RoleAlreadyAssigned);
        }

        var userRole = ApplicationUserRole.Create(
            application.Id,
            request.UserId,
            request.RoleId);

        await _userRoleRepository.AddAsync(
            userRole,
            cancellationToken);

        return Result<AssignApplicationRoleResponse>.Success(
            new AssignApplicationRoleResponse(
                userRole.ApplicationId,
                userRole.UserId,
                userRole.RoleId,
                userRole.CreatedAt));
    }
}