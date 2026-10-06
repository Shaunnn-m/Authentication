using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.Persistence.Configurations;

public sealed class ApplicationConfiguration
    : IEntityTypeConfiguration<Authentication.Domain.Applications.Application>
{
    public void Configure(
        EntityTypeBuilder<Authentication.Domain.Applications.Application> builder)
    {
        builder.ToTable("Applications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ClientId)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.ClientId)
            .IsUnique();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();
    }
}
