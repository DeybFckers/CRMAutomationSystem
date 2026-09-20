using CRMSystem.Models.DTOs;
using CRMSystem.Models.Entities;
using CRMSystem.Repositories.Interface;
using CRMSystem.Services.Interface;
using Mapster;

namespace CRMSystem.Services.Implementation
{
    public class CustomerAddressServices : ICustomerAddressServices
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly ICurrentUserServices _currentUserServices;

        public CustomerAddressServices(ICustomerAddressRepository customerAddressRepository, ICurrentUserServices currentUserServices)
        {
            _customerAddressRepository = customerAddressRepository;
            _currentUserServices = currentUserServices;
        }

        public async Task<IEnumerable<CustomerAddressResponseDto>> GetCustomerAddressByCustomerId(Guid customerId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var customer = await _customerAddressRepository.GetCustomerById(customerId, organizationId);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            var customerAddresses = await _customerAddressRepository.GetCustomerAddressByCustomerId(customerId, organizationId);

            return customerAddresses.Select(ca => ca.Adapt<CustomerAddressResponseDto>());
        }

        public async Task CreateAddress(Guid customerId, CreateCustomerAddressDto customerAddressDto)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var customer = await _customerAddressRepository.GetCustomerById(customerId, organizationId);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            var customerAddress = customerAddressDto.Adapt<CustomerAddress>();

            customerAddress.Id = Guid.NewGuid();
            customerAddress.CustomerId = customer.Id;

            await _customerAddressRepository.CreateAddress(customerAddress);
        }

        public async Task UpdateAddress(Guid customerId, Guid addressId, UpdateCustomerAddressDto customerAddressDto)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var existingAddress = await _customerAddressRepository.GetAddressById(addressId, customerId, organizationId);

            if (existingAddress == null)
                throw new KeyNotFoundException("Customer address not found.");

            customerAddressDto.Adapt(existingAddress);

            await _customerAddressRepository.UpdateAddress(existingAddress);
        }

        public async Task DeleteAddress(Guid customerId, Guid addressId)
        {
            var organizationId = _currentUserServices.OrganizationId;

            var existingAddress = await _customerAddressRepository.GetAddressById(addressId, customerId, organizationId);

            if (existingAddress == null)
                throw new KeyNotFoundException("Customer address not found.");

            await _customerAddressRepository.DeleteAddress(existingAddress);
        }
    }
}