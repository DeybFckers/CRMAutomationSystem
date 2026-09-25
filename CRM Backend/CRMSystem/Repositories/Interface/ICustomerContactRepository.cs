using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ICustomerContactRepository
    {
        Task<Customer?> GetCustomerById(Guid customerId, Guid organizationId);
        Task<CustomerContact?> GetCustomerContactByCustomerId(Guid customerId, Guid organizationId);
        Task<CustomerContact?> GetContactById(Guid contactId, Guid customerId, Guid organizationId);
        Task CreateContact(CustomerContact customerContact);
        Task UpdateContact(CustomerContact customerContact);
        Task DeleteContact(CustomerContact customerContact);
    }
}
