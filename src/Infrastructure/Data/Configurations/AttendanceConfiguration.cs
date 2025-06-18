using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            // Define the table that the entity maps to
            builder.ToTable("Attendance");

            // Define the primary key
            builder.HasKey(a => a.Id);

            // Define the relationship with the Employee entity
            builder.HasOne(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Define the required properties
            builder.Property(a => a.Date)
                .IsRequired();

            builder.Property(a => a.IsPresent)
                .IsRequired();

            // Define the optional properties
            builder.Property(a => a.StartTime)
                .IsRequired(false);

            builder.Property(a => a.EndTime)
                .IsRequired(false);

            builder.Property(a => a.Remarks)
                .IsRequired(false)
                .HasMaxLength(256);
        }
    }
}
