using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class UserPermissionConfig : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        // Composite key
        builder.HasKey(up => new { up.UserId, up.PermissionId });

        // Relationship to Permission
        builder.HasOne(up => up.Permission)
               .WithMany()
               .HasForeignKey(up => up.PermissionId);

        // Relationship to ApplicationUser
        builder.HasOne<ApplicationUser>()
               .WithMany(u => u.UserPermissions)
               .HasForeignKey(up => up.UserId);
    }
}
