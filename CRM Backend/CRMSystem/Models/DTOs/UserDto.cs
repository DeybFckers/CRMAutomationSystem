
namespace CRMSystem.Models.DTOs
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string Status { get; set; } = null!;

        public List<string> Roles { get; set;  } = null!;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateUserDto
    {
        public Guid OrganizationId { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }

    public class UpdateUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = null!;
        public string Status { get; set; } = "ACTIVE";
    }
}
