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

        public async Task<Customer?> GetCustomerIdForCreateAddress(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);   
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

        public async Task DeleteAddress(Guid addressId)
        {
            var address = await _context.CustomerAddresses
                .FirstOrDefaultAsync(x => x.Id == addressId);

            if (address == null)
                return;

            _context.CustomerAddresses.Remove(address);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CustomerAddress>> GetCustomerAddressByCustomerId(Guid id)
        {
            //this query will return the address by using customer id
            return await _context.CustomerAddresses
                .Where(x => x.CustomerId == id)
                .ToListAsync();
        }

        public async Task<CustomerAddress?> GetAddressById(Guid addressId)
        {
            return await _context.CustomerAddresses
                .FirstOrDefaultAsync(x => x.Id == addressId);
        }
    }
}