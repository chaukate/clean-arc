using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Persistence.Configurations
{
    public class UserActivityLogConfiguration : BaseConfiguration<UserActivityLog>
    {
        public override void Configure(EntityTypeBuilder<UserActivityLog> builder)
        {
            base.Configure(builder);

            builder.HasOne(h => h.UserProfileRef)
                .WithMany()
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Url)
                .IsRequired(false);

            builder.Property(p => p.Comment)
                .IsRequired(false);
        }
    }
}
