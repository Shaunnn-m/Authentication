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

    public GetApplicationRolesQueryHandler(
        IApplicationRepository applicationRepository,
        IApplicationRoleRepository roleRepository)
    {
        _applicationRepository = applicationRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Result<GetApplicationRolesResponse>> Handle(
        GetApplicationRolesQuery request,
        CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetByAppplicationId(
            request.ApplicationId,
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
