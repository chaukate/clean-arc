using App.Domain.Entities;
using App.Infrastructure.Persistence.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Persistence.Initializers
{
    public class UserInitializer : BaseInitializer
    {
        public UserInitializer(ModelBuilder modelBuilder) : base(modelBuilder) { }

        public const int SUPER_ADMIN_ID = 1;
        public string SUPER_ADMIN_EMAIL = "";
        public string SUPER_ADMIN_USERNAME = "";
        public string SUPER_ADMIN_FIRSTNAME = "";
        public string SUPER_ADMIN_LASTNAME = "";
        public void SeedUsers()
        {
            var lastUpdatedAt = new DateTimeOffset(new DateTime(2023, 1, 11, 10, 10, 58, 959, DateTimeKind.Unspecified).AddTicks(6954), new TimeSpan(0, 0, 0, 0, 0));

            var dbSuerAgentUser = new User { Id = SUPER_ADMIN_ID, UserName = SUPER_ADMIN_USERNAME, NormalizedUserName = SUPER_ADMIN_USERNAME.ToUpper(), Email = SUPER_ADMIN_EMAIL, NormalizedEmail = SUPER_ADMIN_EMAIL.ToUpper(), EmailConfirmed = true, ConcurrencyStamp = "b91d74d8-5516-4cfb-b7e5-02d3885cb2bd", SecurityStamp = "851536ae-aded-4c9d-b342-738d0fb066eb", LastUpdatedAt = lastUpdatedAt, LastUpdatedBy = "SA", IsActive = true };
            _modelBuilder.Entity<User>().HasData(dbSuerAgentUser);

            var dbSuperAgentProfile = new UserProfile { Id = SUPER_ADMIN_ID, FirstName = SUPER_ADMIN_FIRSTNAME, LastName = SUPER_ADMIN_LASTNAME, LastUpdatedAt = lastUpdatedAt, LastUpdatedBy = "SA", IsActive = true };
            _modelBuilder.Entity<UserProfile>().HasData(dbSuperAgentProfile);

            var dbUserRole = new UserRole { UserId = SUPER_ADMIN_ID, RoleId = RoleInitializer.SUPER_ADMIN };
            _modelBuilder.Entity<UserRole>().HasData(dbUserRole);
        }
    }
}
