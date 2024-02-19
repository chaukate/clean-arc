using App.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Persistence.Configurations
{
    public abstract class BaseUserConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IUser
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(p => p.Id)
                .UseIdentityColumn(1, 1)
                .ValueGeneratedOnAdd()
               .IsRequired();
            builder.HasKey(h => h.Id);

            builder.Property(p => p.Email)
                .HasMaxLength(300)
                .IsRequired(true);
            builder.HasIndex(h => h.Email)
                .IsUnique();
        }
    }
}
