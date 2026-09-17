namespace CRMSystem.Models.Entities
{
    public class CustomerAddress
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string Type { get; set; } = null!;

        public string AddressLine { get; set; } = null!;

        public string City { get; set; } = null!;

        public string? Province { get; set; }

        public string? PostalCode { get; set; }

        public string Country { get; set; } = "Philippines";

        public bool IsPrimary { get; set; }

        // Navigation
        public Customer Customer { get; set; } = null!;
    }
}
