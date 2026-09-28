using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface ICustomerAddressServices
    {
        Task<IEnumerable<CustomerAddressResponseDto>> GetCustomerAddressByCustomerId(Guid customerId);
        Task<CustomerAddressResponseDto> CreateAddress(Guid customerId, CreateCustomerAddressDto customerAddressDto);
        Task<CustomerAddressResponseDto> UpdateAddress(Guid customerId, Guid addressId, UpdateCustomerAddressDto customerAddressDto);
        Task DeleteAddress(Guid customerId, Guid addressId);

    }
}
