using Authentication.Application.Abstractions.Results.Application;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using Authentication.Domain.Applications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Features.Application.RegisterApplicationRole
{
    public sealed class RegisterApplicationRoleCommandHandler
    : IRequestHandler<
        RegisterApplicationRoleCommand,
        RegisterApplicationRoleResult>
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

        public async Task<RegisterApplicationRoleResult> Handle(
            RegisterApplicationRoleCommand request,
            CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.GetByAppplicationId(
                request.ApplicationId,
                cancellationToken);

            if (application is null)
            {
                throw new KeyNotFoundException(
                    "Application was not found.");
            }

            if (!application.IsActive)
            {
                throw new InvalidOperationException(
                    "Application is inactive.");
            }

            var roleExists = await _roleRepository.ExistsAsync(
                request.ApplicationId,
                request.Name,
                cancellationToken);

            if (roleExists)
            {
                throw new InvalidOperationException(
                    "A role with this name already exists for the application.");
            }

            var role = ApplicationRole.Create(
                request.ApplicationId,
                request.Name.Trim());

            await _roleRepository.AddAsync(
                role,
                cancellationToken);

            return new RegisterApplicationRoleResult(
                role.Id,
                role.ApplicationId,
                role.Name);
        }
    }
}
