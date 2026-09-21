using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/customers/{customerId:guid}/contact")]
    public class CustomerContactController : ControllerBase
    {
        private readonly ICustomerContactServices _customerContactServices;

        public CustomerContactController(ICustomerContactServices customerContactServices)
        {
            _customerContactServices = customerContactServices;
        }
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
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
        
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost]
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
        
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPut("{contactId:guid}")]
        public async Task<IActionResult> UpdateCustomerContact(Guid customerId, Guid contactId, UpdateCustomerContactDto dto)
        {
            try
            {
                await _customerContactServices.UpdateCustomerContact(customerId, contactId, dto);

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
        
        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{contactId:guid}")]
        public async Task<IActionResult> DeleteCustomerContact(Guid customerId, Guid contactId)
        {
            try
            {
                await _customerContactServices.DeleteCustomerContact(customerId, contactId);

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