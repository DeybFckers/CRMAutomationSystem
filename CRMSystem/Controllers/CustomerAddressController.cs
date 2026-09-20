using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerAddressController : ControllerBase
    {
        private readonly ICustomerAddressServices _customerAddressServices;

        public CustomerAddressController(ICustomerAddressServices customerAddressServices)
        {
            _customerAddressServices = customerAddressServices;
        }

        [HttpGet("{customerId:guid}")]
        public async Task<IActionResult> GetCustomerAddressById(Guid customerId)
        {
            try
            {
                var address = await _customerAddressServices.GetCustomerAddressByCustomerId(customerId);

                return Ok(address);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("{customerId:guid}")]
        public async Task<IActionResult> CreateAddress(Guid customerId, CreateCustomerAddressDto dto)
        {
            await _customerAddressServices.CreateAddress(customerId, dto);

            return Ok(new
            {
                message = "Customer address created successfully."
            });
        }

        [HttpPut("{addressId:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid addressId, UpdateCustomerAddressDto customerAddressDto)
        {
            try
            {
                await _customerAddressServices.UpdateAddress(addressId, customerAddressDto);

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

        [HttpDelete("{addressId:guid}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            try
            {
                await _customerAddressServices.DeleteAddress(addressId);

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