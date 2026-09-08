using Authentication.Domain.Entities;
using Authentication.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.Persistence;

public class AuthenticationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public AuthenticationDbContext(
        DbContextOptions<AuthenticationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(AuthenticationDbContext).Assembly);
    }
}