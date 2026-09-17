using CRMSystem.Models.DTOs;

namespace CRMSystem.Services.Interface
{
    public interface ICustomerServices
    {
        Task<IEnumerable<CustomerResponseDto>> GetAllCustomer();
        Task<CustomerResponseDto> GetCustomerById(Guid id);
        Task CreateCustomer(CreateCustomerDto customer);
        Task<CustomerResponseDto> UpdateCustomer(Guid id, UpdateCustomerDto customer);
        Task DeleteCustomer(Guid id);
    }
}