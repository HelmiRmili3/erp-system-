//using Backend.Domain.Entities;
//using Backend.Domain.Enums;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Backend.Infrastructure.Data.Configurations;

//public class AbsenceConfiguration : IEntityTypeConfiguration<Absence>
//{
//    public void Configure(EntityTypeBuilder<Absence> builder)
//    {
//        builder.ToTable("Absences");

//        builder.Property(a => a.StartDate)
//            .IsRequired();

//        builder.Property(a => a.EndDate)
//            .IsRequired();

//        builder.Property(a => a.AbsenceType)
//            .IsRequired()
//            .HasMaxLength(50);

//        builder.Property(a => a.StatusType)
//            .IsRequired()
//            .HasMaxLength(20);

//        builder.Property(a => a.Reason)
//            .HasMaxLength(500);

      
      
//        // Indexes for performance
//        builder.HasIndex(a => a.EmployeeId);
//        builder.HasIndex(a => a.StartDate);
//        builder.HasIndex(a => a.EndDate);
//        builder.HasIndex(a => a.StatusType);
//        builder.HasIndex(a => new { a.EmployeeId, a.StartDate, a.EndDate });

//        // Relationship with Employee
//        builder.HasOne(a => a.Employee)
//            .WithMany(e => e.Absences)
//            .HasForeignKey(a => a.EmployeeId)
//            .OnDelete(DeleteBehavior.Cascade);
//    }
//}
