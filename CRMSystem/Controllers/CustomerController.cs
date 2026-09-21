using CRMSystem.Models.DTOs;
using CRMSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRMSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerServices _customerServices;

        public CustomerController(ICustomerServices customerServices)
        {
            _customerServices = customerServices;
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet]
        public async Task<IActionResult> GetAllCustomer()
        {
            var customers = await _customerServices.GetAllCustomer();
            return Ok(customers);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support, Viewer")]
        [HttpGet("{customerId:guid}")]
        public async Task<IActionResult> GetCustomerById(Guid customerId)
        {
            var customer = await _customerServices.GetCustomerById(customerId);
            return Ok(customer);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto customer)
        {
            await _customerServices.CreateCustomer(customer);
            return Ok(new { message = "Customer created successfully." });
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager, SalesRep, Support")]
        [HttpPut("{customerId:guid}")]
        public async Task<IActionResult> UpdateCustomer(Guid customerId, UpdateCustomerDto customer)
        {
            var updatedCustomer = await _customerServices.UpdateCustomer(customerId, customer);
            return Ok(updatedCustomer);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{customerId:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            await _customerServices.DeleteCustomer(customerId);
            return Ok(new { message = "Customer deleted successfully." });
        }
    }
}