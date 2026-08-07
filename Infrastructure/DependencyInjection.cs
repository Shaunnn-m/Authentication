using Authentication.Api.Application.Abstractions.Authentication;
using Authentication.Api.Application.Abstractions.Persistence;
using Authentication.Api.Infrastructure.Persistence.Repositories;
using Authentication.Api.Infrastructure.Persistence;
using Authentication.Api.Infrastructure.Authentication;
using Authentication.Api.Infrastructure.Data;
using Authentication.Api.Infrastructure.Configurations.Authentication;
using Microsoft.EntityFrameworkCore;


namespace Authentication.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });

        services.Configure<PasswordOptions>(
            configuration.GetSection(PasswordOptions.SectionName));

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IPasswordPolicy, PasswordPolicy>();

        return services;
    }
}