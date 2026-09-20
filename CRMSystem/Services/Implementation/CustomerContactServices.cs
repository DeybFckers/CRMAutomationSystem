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

        public CustomerContactServices(ICustomerContactRepository customerContactRepository)
        {
            _customerContactRepository = customerContactRepository;
        }

        public async Task<CustomerContactResponseDto> GetCustomerContactByCustomerId(Guid customerId)
        {
            var contact = await _customerContactRepository.GetCustomerContactByCustomerId(customerId);

            if (contact == null)
                throw new KeyNotFoundException("Customer contact not found.");

            return contact.Adapt<CustomerContactResponseDto>();
        }

        public async Task CreateCustomerContact(Guid customerId, CreateCustomerContactDto dto)
        {
            var customer = await _customerContactRepository.GetCustomerForCreateContact(customerId);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            var existingContact = await _customerContactRepository.GetCustomerContactByCustomerId(customerId);

            if (existingContact != null)
                throw new InvalidOperationException("Customer already has a contact.");

            var customerContact = dto.Adapt<CustomerContact>();

            customerContact.Id = Guid.NewGuid();
            customerContact.CustomerId = customerId;

            await _customerContactRepository.CreateContact(customerContact);
        }

        public async Task UpdateCustomerContact(Guid contactId, UpdateCustomerContactDto dto)
        {
            var contact = await _customerContactRepository.GetContactById(contactId);

            if (contact == null)
                throw new KeyNotFoundException("Customer contact not found.");

            dto.Adapt(contact);

            await _customerContactRepository.UpdateContact(contact);
        }

        public async Task DeleteCustomerContact(Guid contactId)
        {
            var contact = await _customerContactRepository.GetContactById(contactId);

            if (contact == null)
                throw new KeyNotFoundException("Customer contact not found.");

            await _customerContactRepository.DeleteContact(contact.Id);
        }
    }
}
