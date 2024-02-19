 namespace App.Domain.Entities
{
    public class CompanyPerson : BaseEntity<int>
    {
        public int CompanyId { get; set; }
        public int PersonId { get; set; }

        public virtual Company CompanyRef { get; set; }
        public virtual UserProfile PersonRef { get; set; }
    }
}
