using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : BaseConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(h => h.Id);
            builder.HasOne(h => h.ProfileRef)
                .WithOne()
                .HasForeignKey<Customer>(h => h.Id)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(p => p.FinCenId)
                .IsRequired(false);
            builder.HasIndex(h => h.FinCenId)
                .IsUnique();

            builder.Property(p => p.Address)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(p => p.Country)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.State)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Zip)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(p => p.DocumentType)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.DocumentNumber)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.JurisdictionCountry)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.State)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.JurisdictionTribal)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(p => p.JurisdictionTribalDescription)
                .HasMaxLength(3000)
                .IsRequired();

            builder.Property(p => p.JurisdictionDocument)
                .IsRequired();

            builder.Property(p => p.LastUpdatedBy)
               .HasMaxLength(300)
               .IsRequired();
        }
    }
}
