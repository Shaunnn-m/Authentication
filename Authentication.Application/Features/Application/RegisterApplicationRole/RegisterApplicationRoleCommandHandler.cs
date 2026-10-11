using Authentication.Application.Common.Results;
using Authentication.Application.Common.Messages;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using Authentication.Domain.Applications;
using MediatR;

namespace Authentication.Application.Features.Application.RegisterApplicationRole
{
    public sealed class RegisterApplicationRoleCommandHandler
    : IRequestHandler<
        RegisterApplicationRoleCommand,
        Result<RegisterApplicationRoleResponse>>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IApplicationRoleRepository _roleRepository;
        private readonly ICurrentApplicationService _currentApplicationService;

        public RegisterApplicationRoleCommandHandler(
            IApplicationRepository applicationRepository,
            IApplicationRoleRepository roleRepository,
            ICurrentApplicationService currentApplicationService
            )
        {
            _applicationRepository = applicationRepository;
            _roleRepository = roleRepository;
            _currentApplicationService = currentApplicationService;
        }

        public async Task<Result<RegisterApplicationRoleResponse>> Handle(
            RegisterApplicationRoleCommand request,
            CancellationToken cancellationToken)
        {
            var applicationId = _currentApplicationService.ApplicationId;

            if (!applicationId.HasValue || applicationId.Value == Guid.Empty)
            {
                return Result<RegisterApplicationRoleResponse>.Failure(
                    ApplicationMessages.ContextRequired);
            }

            var application = await _applicationRepository.GetByAppplicationId(
                applicationId.Value,
                cancellationToken);

            if (application is null)
            {
                return Result<RegisterApplicationRoleResponse>.Failure(
                    ApplicationMessages.NotFound);
            }

            if (!application.IsActive)
            {
                return Result<RegisterApplicationRoleResponse>.Failure(
                    ApplicationMessages.Inactive);
            }

            var roleName = request.Name.Trim();
            var roleExists = await _roleRepository.ExistsAsync(
                application.Id,
                roleName,
                cancellationToken);

            if (roleExists)
            {
                return Result<RegisterApplicationRoleResponse>.Failure(
                    ApplicationMessages.RoleNameAlreadyExists);
            }

            var role = ApplicationRole.Create(
                application.Id,
                roleName);

            await _roleRepository.AddAsync(
                role,
                cancellationToken);

            return Result<RegisterApplicationRoleResponse>.Success(
                new RegisterApplicationRoleResponse(
                    role.Id,
                    role.ApplicationId,
                    role.Name));
        }
    }
}
