using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ICustomerAddressRepository
    {
        Task<Customer?> GetCustomerById(Guid customerId, Guid organizationId);
        Task<IEnumerable<CustomerAddress>> GetCustomerAddressByCustomerId(Guid customerId, Guid organizationId);
        Task<CustomerAddress?> GetAddressById(Guid addressId, Guid customerId, Guid organizationId);
        Task CreateAddress(CustomerAddress customerAddress);
        Task UpdateAddress(CustomerAddress customerAddress);
        Task DeleteAddress(CustomerAddress customerAddress);
    }
}
