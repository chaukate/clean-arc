using App.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace App.Infrastructure.Persistence.Identity
{
    public class Role : IdentityRole<int>
    {
        public int Priority { get; set; }
        public int? ParentId { get; set; }

        public virtual Role ParentRef { get; set; }
    }
}
