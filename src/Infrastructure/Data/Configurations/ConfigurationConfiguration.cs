using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations;

public class ConfigurationConfiguration : IEntityTypeConfiguration<Configuration>
{
    public void Configure(EntityTypeBuilder<Configuration> builder)
    {
        builder.HasIndex(e => e.Key)
            .IsUnique();

        builder.Property(c => c.Key)
            .IsRequired();

        builder.Property(c => c.Value)
            .IsRequired();
    }
}
