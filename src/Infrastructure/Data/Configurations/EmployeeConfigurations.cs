using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
    
        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.JobTitle)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Department)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.HireDate)
            .IsRequired();

        builder.Property(e => e.ContractType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(50);

    }
}
