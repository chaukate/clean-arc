using App.Domain.Entities;
using App.Domain.Enumerations;
using App.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Infrastructure.Persistence.Identity
{
    public class User : IdentityUser<int>, IUser, ICreatedEvent, IUpdatedEvent
    {
        public bool IsActive { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }

        [NotMapped]
        public UserEventActivity EventActivity { get; set; }
        [NotMapped]
        public string Link { get; set; }

        public virtual UserProfile ProfileRef { get; set; }
        public virtual ICollection<UserRole> Roles { get; set; }
    }
}
