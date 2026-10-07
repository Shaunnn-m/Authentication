using Authentication.Application.Interfaces.Application;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Authentication.Infrastructure.Services.Application
{
    public sealed class CurrentApplicationService
    : ICurrentApplicationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentApplicationService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? ApplicationId
        {
            get
            {
                var value = _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirstValue("application_id");

                return Guid.TryParse(value, out var applicationId)
                    ? applicationId
                    : null;
            }
        }
    }
}
