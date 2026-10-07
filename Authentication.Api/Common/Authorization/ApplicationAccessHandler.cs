using Authentication.Application.Interfaces.Applications;
using Microsoft.AspNetCore.Authorization;

namespace Authentication.Api.Common.Authorization
{
    public sealed class ApplicationAccessHandler
    : AuthorizationHandler<ApplicationAccessRequirement>
    {
        private readonly IApplicationUserAccessRepository _accessRepository;

        public ApplicationAccessHandler(
            IApplicationUserAccessRepository accessRepository)
        {
            _accessRepository = accessRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ApplicationAccessRequirement requirement)
        {
            var userIdValue = context.User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var applicationIdValue = context.User.FindFirst(
                "application_id")?.Value;

            if (!Guid.TryParse(userIdValue, out var userId) ||
                !Guid.TryParse(applicationIdValue, out var applicationId))
            {
                return;
            }

            var hasAccess = await _accessRepository.ExistsAsync(
                applicationId,
                userId);

            if (hasAccess)
            {
                context.Succeed(requirement);
            }
        }
    }
}
