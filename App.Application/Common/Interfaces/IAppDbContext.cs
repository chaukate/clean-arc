using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace App.Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Company> Companies { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<CompanyPerson> CompanyPeople { get; set; }
        DbSet<UserProfile> Profiles { get; set; }
        DbSet<UserActivityLog> UserActivityLogs { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        ChangeTracker ChangeTracker { get; }
    }
}
