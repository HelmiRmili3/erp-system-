using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations;

public class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
{
    public void Configure(EntityTypeBuilder<Payroll> builder)
    {
        builder.ToTable("Payrolls");

        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties
        builder.Property(p => p.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Period)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.BaseSalary)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.Bonuses)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.Deductions)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.NetSalary)
            .IsRequired()
            .HasColumnType("decimal(18, 2)");

        builder.Property(p => p.FileUrl)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(p => p.IsViewedByEmployee)
            .IsRequired()
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade); // or Cascade if you prefer
    }
}
