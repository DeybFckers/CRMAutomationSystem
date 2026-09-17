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
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _customerServices.GetCustomerById(id);
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
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCustomer(Guid id, UpdateCustomerDto customer)
        {
            var updatedCustomer = await _customerServices.UpdateCustomer(id, customer);
            return Ok(updatedCustomer);
        }

        [Authorize(Roles = "SuperAdmin, Admin, SalesManager")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            await _customerServices.DeleteCustomer(id);
            return Ok(new { message = "Customer deleted successfully." });
        }
    }
}