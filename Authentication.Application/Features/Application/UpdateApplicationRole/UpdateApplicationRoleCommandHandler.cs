using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using MediatR;

namespace Authentication.Application.Features.Application.UpdateApplicationRole;

public sealed class UpdateApplicationRoleCommandHandler
    : IRequestHandler<UpdateApplicationRoleCommand, Result<ApplicationRoleResponse>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationRoleRepository _roleRepository;

    public UpdateApplicationRoleCommandHandler(
        IApplicationRepository applicationRepository,
        IApplicationRoleRepository roleRepository)
    {
        _applicationRepository = applicationRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result<ApplicationRoleResponse>> Handle(
        UpdateApplicationRoleCommand request,
        CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetByAppplicationId(
            request.ApplicationId,
            cancellationToken);

        if (application is null)
        {
            return Result<ApplicationRoleResponse>.Failure(
                ApplicationMessages.NotFound);
        }

        if (!application.IsActive)
        {
            return Result<ApplicationRoleResponse>.Failure(
                ApplicationMessages.Inactive);
        }

        var role = await _roleRepository.GetByIdAsync(
            request.RoleId,
            cancellationToken);

        if (role is null || role.ApplicationId != application.Id)
        {
            return Result<ApplicationRoleResponse>.Failure(
                ApplicationMessages.RoleNotFound);
        }

        if (!role.IsActive)
        {
            return Result<ApplicationRoleResponse>.Failure(
                ApplicationMessages.RoleInactive);
        }

        var roleName = request.Name.Trim();
        var roleWithName = await _roleRepository.GetByNameAsync(
            application.Id,
            roleName,
            cancellationToken);

        if (roleWithName is not null && roleWithName.Id != role.Id)
        {
            return Result<ApplicationRoleResponse>.Failure(
                ApplicationMessages.RoleNameAlreadyExists);
        }

        role.Rename(roleName);

        return Result<ApplicationRoleResponse>.Success(
            new ApplicationRoleResponse(
                role.Id,
                role.ApplicationId,
                role.Name,
                role.IsActive));
    }
}
