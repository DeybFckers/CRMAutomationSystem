using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/customers/{customerId:guid}/contact")]
    public class CustomerContactController : BaseController
    {
        private readonly ICustomerContactServices _customerContactServices;

        public CustomerContactController(ICustomerContactServices customerContactServices)
        {
            _customerContactServices = customerContactServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerContact(Guid customerId)
        {
            var contact = await _customerContactServices.GetCustomerContactByCustomerId(customerId);
            return Success("Customer contact retrieved successfully.", contact);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomerContact(Guid customerId, CreateCustomerContactDto dto)
        {
            await _customerContactServices.CreateCustomerContact(customerId, dto);
            return Created("Customer contact created successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPut("{contactId:guid}")]
        public async Task<IActionResult> UpdateCustomerContact(Guid customerId, Guid contactId, UpdateCustomerContactDto dto)
        {
            await _customerContactServices.UpdateCustomerContact(customerId, contactId, dto);
            return Success("Customer contact updated successfully.");
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{contactId:guid}")]
        public async Task<IActionResult> DeleteCustomerContact(Guid customerId, Guid contactId)
        {
            await _customerContactServices.DeleteCustomerContact(customerId, contactId);
            return Success("Customer contact deleted successfully.");
        }
    }
}