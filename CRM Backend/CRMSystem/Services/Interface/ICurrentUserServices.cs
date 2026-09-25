namespace CRMSystem.Services.Interface
{
    public interface ICurrentUserServices
    {
        Guid UserId { get; }
        Guid OrganizationId { get; }
        string? Role { get; }

    }
}
