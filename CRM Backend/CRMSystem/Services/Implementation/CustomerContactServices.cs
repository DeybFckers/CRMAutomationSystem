using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class CustomerContactServices : ICustomerContactServices
    {
        private readonly ICustomerContactRepository _customerContactRepository;
        private readonly ICurrentUserServices _currentUserServices;

        public CustomerContactServices(ICustomerContactRepository customerContactRepository, ICurrentUserServices currentUserServices)
        {
            _customerContactRepository = customerContactRepository;
            _currentUserServices = currentUserServices;
        }

        public async Task<CustomerContactResponseDto> GetCustomerContactByCustomerId(Guid customerId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var contact = await _customerContactRepository.GetCustomerContactByCustomerId(customerId, organizationId);

            if (contact == null)
                throw new KeyNotFoundException("Customer contact not found.");

            return contact.Adapt<CustomerContactResponseDto>();
        }

        public async Task CreateCustomerContact(Guid customerId, CreateCustomerContactDto dto)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var customer = await _customerContactRepository.GetCustomerById(customerId, organizationId);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            var existingContact = await _customerContactRepository.GetCustomerContactByCustomerId(customerId, organizationId);

            if (existingContact != null)
                throw new InvalidOperationException("Customer already has a contact.");

            var customerContact = dto.Adapt<CustomerContact>();

            customerContact.Id = Guid.NewGuid();
            customerContact.CustomerId = customerId;

            await _customerContactRepository.CreateContact(customerContact);
        }

        public async Task UpdateCustomerContact(Guid customerId, Guid contactId, UpdateCustomerContactDto dto)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var contact = await _customerContactRepository.GetContactById(contactId, customerId, organizationId);

            if (contact == null)
                throw new KeyNotFoundException("Customer contact not found.");

            dto.Adapt(contact);

            await _customerContactRepository.UpdateContact(contact);
        }

        public async Task DeleteCustomerContact(Guid customerId, Guid contactId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var contact = await _customerContactRepository.GetContactById(contactId, customerId, organizationId);

            if (contact == null)
                throw new KeyNotFoundException("Customer contact not found.");

            await _customerContactRepository.DeleteContact(contact);
        }
    }
}