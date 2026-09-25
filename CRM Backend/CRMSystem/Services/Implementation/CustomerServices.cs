using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Implementation;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICurrentUserServices _currentUser;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ICustomerCodeServices _customerCodeServices;

        public CustomerServices(ICustomerRepository customerRepository, ICurrentUserServices currentUser, IOrganizationRepository organizationRepository, ICustomerCodeServices customerCodeServices)
        {
            _customerRepository = customerRepository;
            _currentUser = currentUser;
            _organizationRepository = organizationRepository;
            _customerCodeServices = customerCodeServices;
        }

        public async Task CreateCustomer(CreateCustomerDto customer)
        {
            var organization = await _organizationRepository.GetOrganizationById(_currentUser.OrganizationId);
            var newCustomer = customer.Adapt<Customer>();

            newCustomer.Id = Guid.NewGuid();
            newCustomer.OrganizationId = _currentUser.OrganizationId;
            newCustomer.CustomerCode = await _customerCodeServices.GenerateCustomerCode(organization.Id, organization.Name, DateTime.UtcNow);
            newCustomer.Status = "ACTIVE";
            newCustomer.CreatedAt = DateTime.UtcNow;
            newCustomer.UpdatedAt = DateTime.UtcNow;

            await _customerRepository.CreateCustomer(newCustomer);
        }

        public async Task<IEnumerable<CustomerResponseDto>> GetAllCustomer()
        {
            var customers = await _customerRepository.GetAllCustomer(_currentUser.OrganizationId);
            return customers.Adapt<IEnumerable<CustomerResponseDto>>();
        }

        public async Task<CustomerResponseDto> GetCustomerById(Guid id)
        {
            var customer = await _customerRepository.GetCustomerById(id, _currentUser.OrganizationId);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            return customer.Adapt<CustomerResponseDto>();
        }

        public async Task<CustomerResponseDto> UpdateCustomer(Guid id, UpdateCustomerDto customer)
        {
            var existingCustomer = await _customerRepository.GetCustomerById(id, _currentUser.OrganizationId);

            if (existingCustomer == null)
                throw new KeyNotFoundException("Customer not found.");

            customer.Adapt(existingCustomer);
            existingCustomer.UpdatedAt = DateTime.UtcNow;

            await _customerRepository.UpdateCustomer(existingCustomer);

            return existingCustomer.Adapt<CustomerResponseDto>();
        }

        public async Task DeleteCustomer(Guid id)
        {
            await _customerRepository.DeleteCustomer(id, _currentUser.OrganizationId);
        }

    }
}