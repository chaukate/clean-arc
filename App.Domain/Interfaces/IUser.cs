using App.Domain.Entities;
using App.Domain.Enumerations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }
        string Email { get; set; }

        [NotMapped]
        UserEventActivity EventActivity { get; set; }
        [NotMapped]
        string Link { get; set; }

        UserProfile ProfileRef { get; set; }
    }
}
