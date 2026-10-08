using Authentication.Domain.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApplicationEntity = Authentication.Domain.Applications.Application;

namespace Authentication.Infrastructure.Persistence.Configurations;

public sealed class ApplicationSettingsConfiguration
    : IEntityTypeConfiguration<ApplicationSettings>
{
    public void Configure(EntityTypeBuilder<ApplicationSettings> builder)
    {
        builder.ToTable("ApplicationSettings");

        builder.HasKey(x => x.ApplicationId);

        builder.Property(x => x.EmailVerificationEnabled)
            .IsRequired();

        builder.Property(x => x.PasswordResetEnabled)
            .IsRequired();

        builder.Property(x => x.AllowRegistration)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne<ApplicationEntity>()
            .WithOne()
            .HasForeignKey<ApplicationSettings>(
                x => x.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
