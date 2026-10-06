using Authentication.Domain.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.Persistence.Configurations;

public sealed class ApplicationUserAccessConfiguration
    : IEntityTypeConfiguration<ApplicationUserAccess>
{
    public void Configure(
        EntityTypeBuilder<ApplicationUserAccess> builder)
    {
        builder.ToTable("ApplicationUserAccess");

        builder.HasKey(x => new
        {
            x.ApplicationId,
            x.UserId
        });

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.UserId);
    }
}