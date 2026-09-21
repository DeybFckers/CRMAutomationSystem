using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/customers/{customerId:guid}/addresses")]
    public class CustomerAddressController : ControllerBase
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
            try
            {
                var addresses = await _customerAddressServices.GetCustomerAddressByCustomerId(customerId);

                return Ok(addresses);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost]
        public async Task<IActionResult> CreateAddress(Guid customerId, CreateCustomerAddressDto dto)
        {
            try
            {
                await _customerAddressServices.CreateAddress(customerId, dto);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    message = "Customer address created successfully."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPut("{addressId:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid customerId, Guid addressId, UpdateCustomerAddressDto dto)
        {
            try
            {
                await _customerAddressServices.UpdateAddress(customerId, addressId, dto);

                return Ok(new
                {
                    message = "Customer address updated successfully."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{addressId:guid}")]
        public async Task<IActionResult> DeleteAddress(Guid customerId, Guid addressId)
        {
            try
            {
                await _customerAddressServices.DeleteAddress(customerId, addressId);

                return Ok(new
                {
                    message = "Customer address deleted successfully."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
    }
}