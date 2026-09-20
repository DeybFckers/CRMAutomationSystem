using CRMSystem.Data;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CRMSystem.Repositories.Implementation
{
    public class CustomerContactRepository : ICustomerContactRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetCustomerById(Guid customerId, Guid organizationId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == customerId && c.OrganizationId == organizationId);
        }

        public async Task<CustomerContact?> GetCustomerContactByCustomerId(Guid customerId, Guid organizationId)
        {
            return await _context.CustomerContacts
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId && c.Customer.OrganizationId == organizationId);
        }

        public async Task CreateContact(CustomerContact customerContact)
        {
            await _context.CustomerContacts.AddAsync(customerContact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContact(CustomerContact customerContact)
        {
            _context.CustomerContacts.Update(customerContact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContact(CustomerContact customerContact)
        {
            _context.CustomerContacts.Remove(customerContact);
            await _context.SaveChangesAsync();
        }

        public async Task<CustomerContact?> GetContactById(Guid contactId, Guid customerId, Guid organizationId)
        {
            return await _context.CustomerContacts
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(c => c.Id == contactId && c.CustomerId == customerId && c.Customer.OrganizationId == organizationId);
        }
    }
}