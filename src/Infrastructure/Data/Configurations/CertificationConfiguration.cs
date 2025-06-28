using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations;

public class CertificationConfiguration : IEntityTypeConfiguration<Certification>
{
    public void Configure(EntityTypeBuilder<Certification> builder)
    {
        builder.ToTable("Certifications");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Authority)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.DateObtained)
            .IsRequired();

        builder.Property(c => c.FileUrl)
            .IsRequired()
            .HasMaxLength(255);

        // Relationships
        builder.HasOne<ApplicationUser>()
           .WithMany()
           .HasForeignKey(a => a.UserId)
           .OnDelete(DeleteBehavior.Cascade); // or Cascade if you prefer
    }
}
