namespace CRMSystem.Models.Entities
{
    public class CustomerContact
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Position { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public bool IsPrimary { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation
        public Customer Customer { get; set; } = null!;
    }
}
