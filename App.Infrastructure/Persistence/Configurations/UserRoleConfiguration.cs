using App.Infrastructure.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Persistence.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasOne(h => h.RoleRef)
                .WithMany()
                .HasForeignKey(h => h.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.UserRef)
                .WithMany(w => w.Roles)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
