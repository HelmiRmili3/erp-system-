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

            builder.Property(a => a.AttendanceDay)
                   .IsRequired();

            builder.Property(e => e.CheckInMethod)
                .HasConversion<int?>();

            builder.Property(e => e.CheckOutMethod)
                  .HasConversion<int?>();

            // Optional CheckIn fields
            builder.Property(a => a.CheckIn)
                   .IsRequired(false);

            builder.Property(a => a.CheckInLatitude)
                   .IsRequired(false);

            builder.Property(a => a.CheckInLongitude)
                   .IsRequired(false);

            builder.Property(a => a.CheckInDeviceId)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(a => a.CheckInIpAddress)
                   .HasMaxLength(45)
                   .IsRequired(false);

            builder.Property(a => a.IsCheckInByAdmin)
                   .IsRequired()
                   .HasDefaultValue(false);


            // Optional CheckOut fields
            builder.Property(a => a.CheckOut)
                   .IsRequired(false);

            builder.Property(a => a.CheckOutLatitude)
                   .IsRequired(false);

            builder.Property(a => a.CheckOutLongitude)
                   .IsRequired(false);

            builder.Property(a => a.CheckOutDeviceId)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(a => a.CheckOutIpAddress)
                   .HasMaxLength(45)
                   .IsRequired(false);

            builder.Property(a => a.IsCheckOutByAdmin)
                   .IsRequired()
                   .HasDefaultValue(false);

            // Foreign key to ApplicationUser
            builder.HasOne<ApplicationUser>()
                   .WithMany()
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
