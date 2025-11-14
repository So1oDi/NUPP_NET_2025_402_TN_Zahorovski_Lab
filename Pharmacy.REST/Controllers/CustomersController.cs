using Microsoft.AspNetCore.Mvc;
using PharmacyApp.Infrastructure.Models;
using PharmacyApp.Infrastructure.Services;
using Pharmacy.REST.Models;

namespace Pharmacy.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICrudServiceAsync<CustomerModel> _customerService;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(
        ICrudServiceAsync<CustomerModel> customerService,
        ILogger<CustomersController> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        try
        {
            var customers = await _customerService.ReadAllAsync();
            var result = customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address
            });
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all customers");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        try
        {
            var customer = await _customerService.ReadAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var result = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer with ID {CustomerId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto createDto)
    {
        try
        {
            var customer = new CustomerModel
            {
                Name = createDto.Name,
                Address = createDto.Address
            };

            var created = await _customerService.CreateAsync(customer);
            if (!created)
            {
                return BadRequest("Failed to create customer");
            }

            var result = new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Address = customer.Address
            };

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, UpdateCustomerDto updateDto)
    {
        try
        {
            var existingCustomer = await _customerService.ReadAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.Name = updateDto.Name;
            existingCustomer.Address = updateDto.Address;

            var updated = await _customerService.UpdateAsync(existingCustomer);
            if (!updated)
            {
                return BadRequest("Failed to update customer");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer with ID {CustomerId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try
        {
            var customer = await _customerService.ReadAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var removed = await _customerService.RemoveAsync(customer);
            if (!removed)
            {
                return BadRequest("Failed to delete customer");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer with ID {CustomerId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}