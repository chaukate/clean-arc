using App.Infrastructure.Persistence.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Initializers
{
    public class RoleInitializer : BaseInitializer
    {
        public RoleInitializer(ModelBuilder modelBuilder) : base(modelBuilder) { }

        public const int SUPER_ADMIN = 1;
        public const int ADMIN = 2;
        public const int CLIENT = 3;
        public const string SUPER_ADMIN_NAME = "Super Admin";
        public const string ADMIN_NAME = "Admin";
        public const string CLIENT_NAME = "Client";
        public void SeedRoles()
        {
            var dbSuperAdmin = new Role { Id = SUPER_ADMIN, Name = SUPER_ADMIN_NAME, ConcurrencyStamp = "647808af-878a-41e5-9d69-5796165214bd", NormalizedName = SUPER_ADMIN_NAME.ToUpper(), Priority = 100 };
            var dbAdmin = new Role { Id = ADMIN, Name = ADMIN_NAME, ConcurrencyStamp = "728a9e22-c246-4734-87e4-6ef7cb4d3290", NormalizedName = ADMIN_NAME.ToUpper(), Priority = 200 };
            var dbClient = new Role { Id = CLIENT, Name = CLIENT_NAME, ConcurrencyStamp = "728a9e22-c246-4734-87e4-6ef7cb4d3290", NormalizedName = CLIENT_NAME.ToUpper(), Priority = 300 };

            _modelBuilder.Entity<Role>().HasData(dbSuperAdmin, dbAdmin, dbClient);
        }
    }
}
