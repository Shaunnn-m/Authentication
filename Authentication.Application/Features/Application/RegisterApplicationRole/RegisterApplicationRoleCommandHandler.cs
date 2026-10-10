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

        public RegisterApplicationRoleCommandHandler(
            IApplicationRepository applicationRepository,
            IApplicationRoleRepository roleRepository
            )
        {
            _applicationRepository = applicationRepository;
            _roleRepository = roleRepository;
        }

        public async Task<Result<RegisterApplicationRoleResponse>> Handle(
            RegisterApplicationRoleCommand request,
            CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.GetByAppplicationId(
                request.ApplicationId,
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
                request.ApplicationId,
                roleName,
                cancellationToken);

            if (roleExists)
            {
                return Result<RegisterApplicationRoleResponse>.Failure(
                    ApplicationMessages.RoleNameAlreadyExists);
            }

            var role = ApplicationRole.Create(
                request.ApplicationId,
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
