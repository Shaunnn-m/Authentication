using Authentication.Application.Common.Authentication;
using Authentication.Application.Common.Authorization;
using Authentication.Application.Interfaces.Application;
using Authentication.Application.Interfaces.Applications;
using Authentication.Application.Interfaces.Authentication;
using Authentication.Application.Interfaces.Common;
using Authentication.Application.Interfaces.Email;
using Authentication.Application.Interfaces.Identity;
using Authentication.Infrastructure.Identity;
using Authentication.Infrastructure.Persistence;
using Authentication.Infrastructure.Persistence.Repositories;
using Authentication.Infrastructure.Repositories;
using Authentication.Infrastructure.Services.Application;
using Authentication.Infrastructure.Services.Common;
using Authentication.Infrastructure.Services.Email;
using Authentication.Infrastructure.Services.Identity;
using Authentication.Infrastructure.Services.RefreshTokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

        services.RemoveAll<IUserStore<ApplicationUser>>();
        services.AddScoped<IUserStore<ApplicationUser>>(serviceProvider =>
        {
            var store = new UserStore<
                ApplicationUser,
                IdentityRole<Guid>,
                AuthenticationDbContext,
                Guid>(
                    serviceProvider.GetRequiredService<AuthenticationDbContext>());

            store.AutoSaveChanges = false;
            return store;
        });

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationLinkService, AuthenticationLinkService>();
        services.AddScoped<SmtpEmailService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IAdminUserService, AdminUserService>();
        services.AddScoped<IApplicationUserAccessRepository, ApplicationUserAccessRepository>();
        services.AddScoped<
            IApplicationUserRoleRepository,
            ApplicationUserRoleRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IApplicationRoleRepository, ApplicationRoleRepository>();

        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

        services.Configure<AuthenticationEmailOptions>(
            configuration.GetSection(
                AuthenticationEmailOptions.SectionName));

        services.Configure<InitialAdminOptions>(
            configuration.GetSection(
                InitialAdminOptions.SectionName));

        services.Configure<SmtpOptions>(
            configuration.GetSection(
                SmtpOptions.SectionName));

        return services;
    }
}
