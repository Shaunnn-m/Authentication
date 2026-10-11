using Authentication.Application.Common.Messages;
using Authentication.Application.Common.Results;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using MediatR;

namespace Authentication.Application.Features.Application.QueryApplicationRoles;

public sealed class GetApplicationRolesQueryHandler
    : IRequestHandler<GetApplicationRolesQuery, Result<GetApplicationRolesResponse>>
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationRoleRepository _roleRepository;
    private readonly ICurrentApplicationService _currentApplicationService;

    public GetApplicationRolesQueryHandler(
        IApplicationRepository applicationRepository,
        IApplicationRoleRepository roleRepository,
        ICurrentApplicationService currentApplicationService)
    {
        _applicationRepository = applicationRepository;
        _roleRepository = roleRepository;
        _currentApplicationService = currentApplicationService;
    }

    public async Task<Result<GetApplicationRolesResponse>> Handle(
        GetApplicationRolesQuery request,
        CancellationToken cancellationToken)
    {
        var applicationId = _currentApplicationService.ApplicationId;

        if (!applicationId.HasValue || applicationId.Value == Guid.Empty)
        {
            return Result<GetApplicationRolesResponse>.Failure(
                ApplicationMessages.ContextRequired);
        }

        var application = await _applicationRepository.GetByAppplicationId(
            applicationId.Value,
            cancellationToken);

        if (application is null)
        {
            return Result<GetApplicationRolesResponse>.Failure(
                ApplicationMessages.NotFound);
        }

        var roles = await _roleRepository.GetByApplicationIdAsync(
            application.Id,
            cancellationToken);

        return Result<GetApplicationRolesResponse>.Success(
            new GetApplicationRolesResponse(
                roles
                    .Select(role => new ApplicationRoleResponse(
                        role.Id,
                        role.ApplicationId,
                        role.Name,
                        role.IsActive))
                    .ToList()));
    }
}
