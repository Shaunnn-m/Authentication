using Authentication.Application.Common.Authentication;
using Authentication.Infrastructure.Identity;
using Authentication.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AuthenticationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AuthenticationDatabase")));

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AuthenticationDbContext>();

        services.AddScoped<ITokenService, JwtTokenService>();

        services.Configure<JwtOptions>(options =>
            configuration.GetSection(JwtOptions.SectionName));

        return services;
    }
}
