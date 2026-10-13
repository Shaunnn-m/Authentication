using Authentication.Domain.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.Persistence.Configurations;

public sealed class ApplicationClientCredentialConfiguration
    : IEntityTypeConfiguration<ApplicationClientCredential>
{
    public void Configure(
        EntityTypeBuilder<ApplicationClientCredential> builder)
    {
        builder.ToTable("ApplicationClientCredentials");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ApplicationId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.SecretHash)
            .HasColumnType("binary(32)")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ExpiresAt);

        builder.Property(x => x.RevokedAt);

        builder.Property(x => x.LastUsedAt);

        builder.HasIndex(x => x.ApplicationId);

        builder.HasOne<Authentication.Domain.Applications.Application>()
            .WithMany()
            .HasForeignKey(x => x.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}