using App.Infrastructure.Persistence.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : BaseUserConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.LastUpdatedBy)
                .HasMaxLength(300)
                .IsRequired(true);
        }
    }
}
