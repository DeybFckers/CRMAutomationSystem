using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface ICustomerContactServices
    {
        Task<CustomerContactResponseDto> GetCustomerContactByCustomerId(Guid customerId);
        Task CreateCustomerContact(Guid customerId, CreateCustomerContactDto dto);
        Task UpdateCustomerContact(Guid customerId, UpdateCustomerContactDto dto);
        Task DeleteCustomerContact(Guid customerId);
    }
}
