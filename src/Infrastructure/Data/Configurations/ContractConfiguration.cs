using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("Contracts");

        // Primary Key
        builder.HasKey(c => c.Id);

        // Properties
        builder.Property(c => c.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ContractType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.StartDate)
            .IsRequired();

        builder.Property(c => c.EndDate)
            .IsRequired(false);

        builder.Property(c => c.FileUrl)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue(EmployeeStatus.Active);

        // Relationships
        builder.HasOne<ApplicationUser>()
           .WithMany()
           .HasForeignKey(a => a.UserId)
           .OnDelete(DeleteBehavior.Cascade); // or Cascade if you prefer
    }
}
