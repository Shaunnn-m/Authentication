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
    private readonly ICurrentApplicationService _currentApplicationService;

    public DeleteApplicationRoleCommandHandler(
        IApplicationRepository applicationRepository,
        IApplicationRoleRepository roleRepository,
        ICurrentApplicationService currentApplicationService)
    {
        _applicationRepository = applicationRepository;
        _roleRepository = roleRepository;
        _currentApplicationService = currentApplicationService;
    }

    public async Task<Result<ApplicationRoleResponse>> Handle(
        DeleteApplicationRoleCommand request,
        CancellationToken cancellationToken)
    {
        var applicationId = _currentApplicationService.ApplicationId;

        if (!applicationId.HasValue || applicationId.Value == Guid.Empty)
        {
            return Result<ApplicationRoleResponse>.Failure(
                ApplicationMessages.ContextRequired);
        }

        var application = await _applicationRepository.GetByAppplicationId(
            applicationId.Value,
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
