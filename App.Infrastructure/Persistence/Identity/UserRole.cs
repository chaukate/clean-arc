using Microsoft.AspNetCore.Identity;

namespace App.Infrastructure.Persistence.Identity
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual User UserRef { get; set; }
        public virtual Role RoleRef { get; set; }
    }
}
