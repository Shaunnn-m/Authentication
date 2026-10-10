using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using MediatR;

namespace Authentication.Application.Features.Application.DeleteApplicationRole;

public sealed class DeleteApplicationRoleCommandHandler
    : IRequestHandler<DeleteApplicationRoleCommand, Result<ApplicationRoleResponse>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationRoleRepository _roleRepository;

    public DeleteApplicationRoleCommandHandler(
        IApplicationRepository applicationRepository,
        IApplicationRoleRepository roleRepository)
    {
        _applicationRepository = applicationRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result<ApplicationRoleResponse>> Handle(
        DeleteApplicationRoleCommand request,
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

        if (role.IsActive)
        {
            role.Deactivate();
        }

        return Result<ApplicationRoleResponse>.Success(
            new ApplicationRoleResponse(
                role.Id,
                role.ApplicationId,
                role.Name,
                role.IsActive));
    }
}
