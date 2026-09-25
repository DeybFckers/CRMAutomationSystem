using CRMSystem.Models.Entities;

namespace CRMSystem.Repositories.Interface
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllCustomer(Guid organizationId);
        Task<Customer?> GetCustomerById(Guid id, Guid organizationId);
        Task CreateCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(Guid id, Guid organizationId);
        Task<int> GetCustomerCountByMonth(Guid organizationId, int year, int month);

        void AddCustomer(Customer customer);
    }
}