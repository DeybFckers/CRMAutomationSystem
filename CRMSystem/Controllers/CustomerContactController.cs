using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerContactController : ControllerBase
    {
        private readonly ICustomerContactServices _customerContactServices;

        public CustomerContactController(ICustomerContactServices customerContactServices)
        {
            _customerContactServices = customerContactServices;
        }

        [HttpGet("{customerId:guid}")]
        public async Task<ActionResult<CustomerContactResponseDto>> GetCustomerContact(Guid customerId)
        {
            try
            {
                var contact = await _customerContactServices.GetCustomerContactByCustomerId(customerId);

                return Ok(contact);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("{customerId:guid}")]
        public async Task<IActionResult> CreateCustomerContact(Guid customerId, CreateCustomerContactDto dto)
        {
            try
            {
                await _customerContactServices.CreateCustomerContact(customerId, dto);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    message = "Customer contact created successfully."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{contactId:guid}")]
        public async Task<IActionResult> UpdateCustomerContact(Guid contactId, UpdateCustomerContactDto dto)
        {
            try
            {
                await _customerContactServices.UpdateCustomerContact(contactId, dto);

                return Ok(new
                {
                    message = "Customer contact updated successfully."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{contactId:guid}")]
        public async Task<IActionResult> DeleteCustomerContact(Guid contactId)
        {
            try
            {
                await _customerContactServices.DeleteCustomerContact(contactId);

                return Ok(new
                {
                    message = "Customer contact deleted successfully."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}