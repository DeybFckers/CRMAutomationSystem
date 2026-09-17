using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ICustomerAddressRepository
    {
        Task<Customer?> GetCustomerIdForCreateAddress(Guid id);
        Task<IEnumerable<CustomerAddress>> GetCustomerAddressByCustomerId(Guid id);
        Task CreateAddress(CustomerAddress customerAddress);

        Task UpdateAddress(CustomerAddress customerAddress);

        Task DeleteAddress(Guid addressId);
        Task<CustomerAddress?> GetAddressById(Guid addressId);
    }
}
