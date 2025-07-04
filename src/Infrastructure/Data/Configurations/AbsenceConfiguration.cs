using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infrastructure.Data.Configurations;

public class AbsenceConfiguration : IEntityTypeConfiguration<Absence>
{
    public void Configure(EntityTypeBuilder<Absence> builder)
    {
        builder.ToTable("Absences");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.StartDate)
            .IsRequired();

        builder.Property(a => a.EndDate)
            .IsRequired();

        builder.Property(a => a.AbsenceType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.StatusType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(a => a.Reason)
            .HasMaxLength(500);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade); // or Cascade if you prefer
    }
}
