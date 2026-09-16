using Authentication.Application.Common.Authentication;
using Authentication.Application.Common.Authorization;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Interfaces.Identity;
using Authentication.Infrastructure.Identity;
using Authentication.Infrastructure.Persistence;
using Authentication.Infrastructure.Services.Email;
using Authentication.Infrastructure.Services.Identity;
using Authentication.Infrastructure.Services.RefreshTokens;
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

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 12;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;

            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<AuthenticationDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationLinkService, AuthenticationLinkService>();
        services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
        services.AddScoped<IEmailService, LoggingEmailService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddScoped<IPasswordService, PasswordService>();

        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

        services.Configure<AuthenticationEmailOptions>(
            configuration.GetSection(
                AuthenticationEmailOptions.SectionName));

        services.Configure<InitialAdminOptions>(
            configuration.GetSection(
                InitialAdminOptions.SectionName));

        return services;
    }
}
