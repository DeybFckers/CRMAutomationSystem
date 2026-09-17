using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface ICustomerAddressServices
    {
        Task<IEnumerable<CustomerAddressResponseDto>> GetCustomerAddressByCustomerId(Guid id);   

        Task CreateAddress(Guid customerId, CreateCustomerAddressDto dto);

        Task UpdateAddress(Guid addressId, UpdateCustomerAddressDto dto);

        Task DeleteAddress(Guid addressId);

    }
}
