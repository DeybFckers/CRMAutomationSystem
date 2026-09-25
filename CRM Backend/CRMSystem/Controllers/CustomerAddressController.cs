using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/customers/{customerId:guid}/addresses")]
    public class CustomerAddressController : BaseController
    {
        private readonly ICustomerAddressServices _customerAddressServices;

        public CustomerAddressController(ICustomerAddressServices customerAddressServices)
        {
            _customerAddressServices = customerAddressServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerAddresses(Guid customerId)
        {
            var addresses = await _customerAddressServices.GetCustomerAddressByCustomerId(customerId);
            return Success("Customer addresses retrieved successfully.", addresses);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost]
        public async Task<IActionResult> CreateAddress(Guid customerId, CreateCustomerAddressDto dto)
        {
            await _customerAddressServices.CreateAddress(customerId, dto);
            return Created("Customer address created successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support")]
        [HttpPut("{addressId:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid customerId, Guid addressId, UpdateCustomerAddressDto dto)
        {
            await _customerAddressServices.UpdateAddress(customerId, addressId, dto);
            return Success("Customer address updated successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{addressId:guid}")]
        public async Task<IActionResult> DeleteAddress(Guid customerId, Guid addressId)
        {
            await _customerAddressServices.DeleteAddress(customerId, addressId);
            return Success("Customer address deleted successfully.");
        }
    }
}