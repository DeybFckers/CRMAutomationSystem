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
        

        public CustomerAddressServices(ICustomerAddressRepository customerAddressRepository)
        {
            _customerAddressRepository = customerAddressRepository;
            
        }
        public async Task<IEnumerable<CustomerAddressResponseDto>> GetCustomerAddressByCustomerId(Guid id)
        {
            var customerAddresses = await _customerAddressRepository.GetCustomerAddressByCustomerId(id);

            if (customerAddresses == null)
                throw new KeyNotFoundException("Customer addresses not found.");

            return customerAddresses.Select(ca => ca.Adapt<CustomerAddressResponseDto>());
        }
        public async Task CreateAddress(Guid id, CreateCustomerAddressDto customerAddressDto)
        {
            var customer = await _customerAddressRepository.GetCustomerIdForCreateAddress(id);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found.");

            var customerAddress = customerAddressDto.Adapt<CustomerAddress>();

            customerAddress.Id = Guid.NewGuid();
            customerAddress.CustomerId = customer.Id;

            await _customerAddressRepository.CreateAddress(customerAddress);
        }
       
        public async Task UpdateAddress(Guid id, UpdateCustomerAddressDto customerAddressDto)
        {

            var existingAddress = await _customerAddressRepository.GetAddressById(id);

            if (existingAddress == null)
                throw new KeyNotFoundException("Customer address not found.");

            customerAddressDto.Adapt(existingAddress);

            await _customerAddressRepository.UpdateAddress(existingAddress);
        }
        
        public async Task DeleteAddress(Guid id)
        {
            var existingAddress = await _customerAddressRepository.GetAddressById(id);

            if (existingAddress == null)
                throw new KeyNotFoundException("Customer address not found.");

            await _customerAddressRepository.DeleteAddress(id);
        }
    }
}