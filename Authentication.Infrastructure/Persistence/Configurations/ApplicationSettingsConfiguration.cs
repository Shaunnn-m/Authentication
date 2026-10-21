using Authentication.Domain.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.Persistence.Configurations;

public sealed class ApplicationSettingsConfiguration
    : IEntityTypeConfiguration<ApplicationSettings>
{
    public void Configure(EntityTypeBuilder<ApplicationSettings> builder)
    {
        builder.ToTable("ApplicationSettings");

        builder.HasKey(settings => settings.ApplicationId);

        builder.Property(settings => settings.IsEmailVerificationEnabled)
            .IsRequired();

        builder.Property(settings => settings.EmailHandlingMode)
            .IsRequired();

        builder.Property(settings => settings.CreatedAt)
            .IsRequired();

        builder.HasOne<Authentication.Domain.Applications.Application>()
            .WithOne()
            .HasForeignKey<ApplicationSettings>(
                settings => settings.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
