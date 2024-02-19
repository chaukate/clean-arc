using App.Domain.Enumerations;

namespace App.Domain.Entities
{
    public class Customer : BaseEntity<int>
    {
        public long? FinCenId { get; set; }
        public bool IsMinor { get; set; }
        public bool IsExempt { get; set; }
        public AddressType AddressType { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public string JurisdictionCountry { get; set; }
        public string JurisdictionState { get; set; }
        public string JurisdictionTribal { get; set; }
        public string JurisdictionTribalDescription { get; set; }
        public byte[] JurisdictionDocument { get; set; }

        public virtual UserProfile ProfileRef { get; set; }

    }
}
