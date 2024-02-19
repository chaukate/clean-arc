using App.Domain.Enumerations;

namespace App.Domain.Entities
{
    public class UserActivityLog : BaseEntity<int>
    {
        public int UserId { get; set; }
        public Area Area { get; set; }
        public Module Module { get; set; }
        public Enumerations.Action Action { get; set; }
        public string Url { get; set; }
        public string Comment { get; set; }

        public virtual UserProfile UserProfileRef { get; set; }
    }
}
