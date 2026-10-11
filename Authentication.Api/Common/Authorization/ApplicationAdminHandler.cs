using Authentication.Application.Common.Authorization;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Authentication.Api.Common.Authorization
{
    public sealed class ApplicationAdminHandler
    : AuthorizationHandler<ApplicationAdminRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationUserAccessRepository _accessRepository;
        private readonly IApplicationUserRoleRepository _userRoleRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly ICurrentApplicationService _currentApplicationService;

        public ApplicationAdminHandler(
            IHttpContextAccessor httpContextAccessor,
            IApplicationUserAccessRepository accessRepository,
            IApplicationUserRoleRepository userRoleRepository,
            IApplicationRepository applicationRepository,
            ICurrentApplicationService currentApplicationService)
        {
            _httpContextAccessor = httpContextAccessor;
            _accessRepository = accessRepository;
            _userRoleRepository = userRoleRepository;
            _applicationRepository = applicationRepository;
            _currentApplicationService = currentApplicationService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ApplicationAdminRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is null)
                return;

            var userIdValue = context.User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
                return;

            if (!Guid.TryParse(
                    httpContext.Request.RouteValues["applicationId"]?.ToString(),
                    out var applicationId))
                return;

            if (_currentApplicationService.ApplicationId != applicationId)
                return;

            var application = await _applicationRepository.GetByAppplicationId(
                applicationId,
                httpContext.RequestAborted);

            if (application is null || !application.IsActive)
                return;

            var hasActiveAccess = await _accessRepository.ExistsAsync(
                applicationId,
                userId,
                httpContext.RequestAborted);

            if (!hasActiveAccess)
                return;

            var roles = await _userRoleRepository.GetRolesAsync(
                applicationId,
                userId,
                httpContext.RequestAborted);

            if (roles.Any(role =>
                    role.IsActive &&
                    string.Equals(
                        role.Name,
                        AppRoles.Admin,
                        StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
        }
    }
}
