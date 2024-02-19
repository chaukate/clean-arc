namespace App.Domain.Entities
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
