using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomer(Guid organizationId)
        {
            return await _context.Customers
                .Include(x => x.Contacts)
                .Include(x => x.Addresses)
                .AsSplitQuery()
                .Where(x => x.OrganizationId == organizationId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerById(Guid id, Guid organizationId)
        {
            return await _context.Customers
                .Include(x => x.Contacts)
                .Include(x => x.Addresses)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);
        }

        public async Task UpdateCustomer(Customer customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(Guid id, Guid organizationId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == organizationId);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetCustomerCountByMonth(Guid organizationId, int year, int month)
        {
            //get the organization name then
            return await _context.Customers                         //year created              //month created
                 .Where(x => x.OrganizationId == organizationId && x.CreatedAt.Year == year && x.CreatedAt.Month == month)
                 .CountAsync();
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
        }
    }
}