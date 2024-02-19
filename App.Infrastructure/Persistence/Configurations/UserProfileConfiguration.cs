using App.Domain.Entities;
using App.Infrastructure.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Persistence.Configurations
{
    public class UserProfileConfiguration : BaseConfiguration<UserProfile>
    {
        public override void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.Property(p => p.Id)
                .IsRequired();
            builder.HasKey(h => h.Id);
            builder.HasOne(h => (User)h.UserRef)
                .WithOne(w => w.ProfileRef)
                .HasForeignKey<UserProfile>(h => h.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Suffix)
                .HasMaxLength(10)
                .IsRequired(false);

            builder.Property(p => p.FirstName)
                .HasMaxLength(300)
                .IsRequired(false);

            builder.Property(p => p.MiddleName)
                .HasMaxLength(300)
                .IsRequired(false);

            builder.Property(p => p.LastName)
                .HasMaxLength(300)
                .IsRequired(false);
        }
    }
}
