namespace App.Domain.Entities
{
    public class Company : BaseEntity<int>
    {
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
