namespace CRMSystem.Services.Interface
{
    public interface ICustomerCodeServices
    {
        Task<string> GenerateCustomerCode(Guid organizationId, string organizationName, DateTime date);
        
    }
}
