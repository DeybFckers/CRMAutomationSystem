namespace CRMSystem.Services.Interface
{
    public interface ICurrentUserServices
    {
        Guid UserId { get; }
        Guid OrganizationId { get; }
        List<string> Roles { get; }

    }
}
