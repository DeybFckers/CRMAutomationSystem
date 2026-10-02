using CRMSystem.Models.DTOs;
using CRMSystem.Models.Responses;

namespace CRMSystem.Services.Interface
{
    public interface ICustomerServices
    {
        Task <PaginatedResponse<CustomerResponseDto>> GetAllCustomer(int page, int pageSize, string? search, string? sortBy, string? sortDirection, Guid? assignedUserId);
        Task<CustomerResponseDto> GetCustomerById(Guid id);
        Task<CustomerResponseDto> CreateCustomer(CreateCustomerDto customer);
        Task<CustomerResponseDto> UpdateCustomer(Guid id, UpdateCustomerDto customer);
        Task DeleteCustomer(Guid id);
    }
}