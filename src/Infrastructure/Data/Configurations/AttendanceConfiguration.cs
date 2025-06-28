using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations
{
    public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendance");

            builder.HasKey(a => a.Id);

 
            // Required
            builder.Property(a => a.UserId)
                   .IsRequired();
         
            builder.Property(a => a.AttendanceDate)
                   .IsRequired();

            builder.Property(a => a.CheckInMethod)
                   .IsRequired()
                   .HasMaxLength(50);

            // Optional
            builder.Property(a => a.CheckIn)
                   .IsRequired(false);

            builder.Property(a => a.CheckOut)
                   .IsRequired(false);

            builder.Property(a => a.IpAddress)
                   .HasMaxLength(45)
                   .IsRequired(false);

            builder.Property(a => a.Latitude)
                   .IsRequired(false);

            builder.Property(a => a.Longitude)
                   .IsRequired(false);

            builder.Property(a => a.DeviceId)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(a => a.Notes)
                   .HasMaxLength(256)
                   .IsRequired(false);
            builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade); // or Cascade if you prefer
        }
    }
}

