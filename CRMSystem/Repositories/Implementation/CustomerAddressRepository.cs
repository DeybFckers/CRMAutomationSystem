using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class CustomerAddressRepository : ICustomerAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerAddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerById(Guid customerId, Guid organizationId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId && c.OrganizationId == organizationId);
        }

        public async Task CreateAddress(CustomerAddress customerAddress)
        {
            await _context.CustomerAddresses.AddAsync(customerAddress);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAddress(CustomerAddress customerAddress)
        {
            _context.CustomerAddresses.Update(customerAddress);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddress(CustomerAddress customerAddress)
        {
            _context.CustomerAddresses.Remove(customerAddress);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerAddress>> GetCustomerAddressByCustomerId(Guid customerId, Guid organizationId)
        {
            return await _context.CustomerAddresses
                .Where(x => x.CustomerId == customerId && x.Customer.OrganizationId == organizationId)
                .ToListAsync();
        }

        public async Task<CustomerAddress?> GetAddressById(Guid addressId, Guid customerId, Guid organizationId)
        {
            return await _context.CustomerAddresses
                .Include(x => x.Customer)
                .FirstOrDefaultAsync(x => x.Id == addressId && x.CustomerId == customerId && x.Customer.OrganizationId == organizationId);
        }
    }
}