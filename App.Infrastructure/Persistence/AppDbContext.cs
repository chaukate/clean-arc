using App.Application.Common.Events;
using App.Application.Common.Interfaces;
using App.Domain.Entities;
using App.Domain.Interfaces;
using App.Infrastructure.Persistence.Identity;
using App.Infrastructure.Persistence.Initializers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace App.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User,
                                                  Role,
                                                  int,
                                                  IdentityUserClaim<int>,
                                                  UserRole,
                                                  IdentityUserLogin<int>,
                                                  IdentityRoleClaim<int>,
                                                  IdentityUserToken<int>>,
                                  IAppDbContext
    {
        private readonly IEventDispatcherService _eventDispatcherService;
        public AppDbContext(DbContextOptions<AppDbContext> options,
                              IEventDispatcherService eventDispatcherService) :
            base(options)
        {
            _eventDispatcherService = eventDispatcherService;
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyPerson> CompanyPeople { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<UserProfile> Profiles { get; set; }
        public DbSet<UserActivityLog> UserActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // always run OnModelCreating before running custom configuration to avoid overwrite of custom navigation on identity user
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(IUser).IsAssignableFrom(entityType.ClrType))
                {
                    builder.Entity(entityType.ClrType).Property<int>(nameof(IUser.Id)).UseIdentityColumn(1, 1).ValueGeneratedOnAdd().IsRequired();
                    builder.Entity(entityType.ClrType).HasKey(nameof(IUser.Id));

                    builder.Entity(entityType.ClrType).Property<string>(nameof(IUser.Email)).HasMaxLength(300).IsRequired();
                    builder.Entity(entityType.ClrType).HasIndex(nameof(IUser.Email)).IsUnique();
                }
            }

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            var roleInitializer = new RoleInitializer(builder);
            roleInitializer.SeedRoles();
            var userInitializer = new UserInitializer(builder);
            userInitializer.SeedUsers();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                QueueDomainEvents();
                var result = await base.SaveChangesAsync(cancellationToken);
                return result;
            }
            catch
            {
                _eventDispatcherService.ClearQueue();
                throw;
            }
        }

        private void QueueDomainEvents()
        {
            var addedEntities = ChangeTracker.Entries<ICreatedEvent>().Where(w => w.State == EntityState.Added);
            foreach (var addedEntity in addedEntities)
            {
                var entity = new CreatedEvent(addedEntity.Entity);
                _eventDispatcherService.QueueNotification(entity);
            }

            var updatedEntities = ChangeTracker.Entries<IUpdatedEvent>().Where(w => w.State == EntityState.Modified);
            foreach (var updatedEntitiy in updatedEntities)
            {
                var entity = new UpdatedEvent(updatedEntitiy.Entity);
                _eventDispatcherService.QueueNotification(entity);
            }
        }
    }
}
