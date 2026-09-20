using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ICustomerContactRepository
    {
        Task<Customer?> GetCustomerForCreateContact(Guid customerId);
        Task<CustomerContact?> GetCustomerContactByCustomerId(Guid customerId);
        Task CreateContact(CustomerContact customerContact);
        Task UpdateContact(CustomerContact customerContact);
        Task DeleteContact(Guid id);
        Task<CustomerContact?> GetContactById(Guid contactId);
    }
}
