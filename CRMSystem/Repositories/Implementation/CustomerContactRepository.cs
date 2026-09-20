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

        public async Task<Customer?> GetCustomerForCreateContact(Guid customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<CustomerContact?> GetCustomerContactByCustomerId(Guid customerId)
        {
            return await _context.CustomerContacts.FirstOrDefaultAsync(c => c.CustomerId == customerId);
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

        public async Task DeleteContact(Guid id)
        {
            var contact = await GetContactById(id);

            if (contact == null)
                return;

            _context.CustomerContacts.Remove(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<CustomerContact?> GetContactById(Guid contactId)
        {
            return await _context.CustomerContacts.FirstOrDefaultAsync(c => c.Id == contactId);
        }
    }
}