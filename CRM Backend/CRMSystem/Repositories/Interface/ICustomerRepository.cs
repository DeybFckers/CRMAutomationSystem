using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ICustomerRepository
    {
        Task<(IEnumerable<Customer> Customers, int TotalCount)> GetAllCustomers(Guid organizationId, int page, int pageSize, string? search, string? sortBy, string? sortDirection, Guid? assignUserId);
        Task<Customer?> GetCustomerById(Guid id, Guid organizationId);
        Task CreateCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(Guid id, Guid organizationId);
        Task<int> GetCustomerCountByMonth(Guid organizationId, int year, int month);

        void AddCustomer(Customer customer);
    }
}