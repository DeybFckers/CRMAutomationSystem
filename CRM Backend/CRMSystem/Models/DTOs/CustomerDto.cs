namespace CRMSystem.Models.DTOs
{
    public class CustomerResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public Guid? AssignedUserId { get; set; }

        public string CustomerCode { get; set; } = null!;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<CustomerContactResponseDto> Contacts { get; set; } = [];
        public List<CustomerAddressResponseDto> Addresses { get; set; } = [];
    }

    public class CreateCustomerDto
    {
        public Guid? AssignedUserId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public class UpdateCustomerDto
    {
        public Guid? AssignedUserId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string Status { get; set; } = "ACTIVE";
    }

    public class CustomerContactResponseDto
    {
        public Guid Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }

        public bool IsPrimary { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CreateCustomerContactDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }

        public bool IsPrimary { get; set; }
    }

    public class UpdateCustomerContactDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }

        public bool IsPrimary { get; set; }
    }

    public class CustomerAddressResponseDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string Country { get; set; } = null!;

        public bool IsPrimary { get; set; }
    }

    public class CreateCustomerAddressDto
    {
        public string Type { get; set; } = null!;
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string Country { get; set; } = "Philippines";
        public bool IsPrimary { get; set; }
    }

    public class UpdateCustomerAddressDto
    {
        public string Type { get; set; } = null!;
        public string AddressLine { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Province { get; set; }
        public string? PostalCode { get; set; }
        public string Country { get; set; } = "Philippines";
        public bool IsPrimary { get; set; }
    }


}
