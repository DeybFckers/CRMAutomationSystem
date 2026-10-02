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

        public async Task<(IEnumerable<Customer> Customers, int TotalCount)> GetAllCustomers(Guid organizationId, int page, int pageSize, string? search, string? sortBy, string? sortDirection, Guid? assignUserId)
        {
            var query = _context.Customers.Where(x => x.OrganizationId == organizationId);

            if (assignUserId.HasValue)
                query = query.Where(x => x.AssignedUserId == assignUserId.Value);


            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();

                query = query.Where(x => 
                (x.FirstName != null && x.FirstName.Contains(search)) ||
                (x.LastName != null && x.LastName.Contains(search)) ||
                (x.CompanyName != null && x.CompanyName.Contains(search)) ||
                (x.Email != null && x.Email.Contains(search)) ||
                (x.Phone != null && x.Phone.Contains(search)) ||
                (x.CustomerCode != null && x.CustomerCode.Contains(search)));
            }
            
            var totalCount = await query.CountAsync();

            sortBy = sortBy?.ToLower();
            sortDirection = sortDirection?.ToLower();

            switch (sortBy)
            {
                case "firstname":
                    query = sortDirection == "asc" 
                        ? query.OrderBy(x => x.FirstName) 
                        : query.OrderByDescending(x => x.FirstName);
                    break;
                case "lastname":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.LastName)
                        : query.OrderByDescending(x => x.LastName);
                    break;
                case "companyname":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.CompanyName)
                        : query.OrderByDescending(x => x.CompanyName);
                    break;
                case "customercode":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.CustomerCode)
                        : query.OrderByDescending(x => x.CustomerCode);
                    break;
                case "email":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.Email)
                        : query.OrderByDescending(x => x.Email);
                    break;
                case "phone":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.Phone)
                        : query.OrderByDescending(x => x.Phone);
                    break;
                case "updatedat":
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.UpdatedAt)
                        : query.OrderByDescending(x => x.UpdatedAt);
                    break;

                case "createdat":
                default:
                    query = sortDirection == "asc"
                        ? query.OrderBy(x => x.CreatedAt)
                        : query.OrderByDescending(x => x.CreatedAt);
                    break;
            }

            var customer = await query
                .Include(x => x.AssignedUser)
                .Include(x => x.Contacts)
                .Include(x => x.Addresses)
                .AsSplitQuery()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return(customer, totalCount);


        }

        public async Task CreateCustomer(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<Customer?> GetCustomerById(Guid id, Guid organizationId)
        {
            return await _context.Customers
                .Include(x => x.AssignedUser)
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