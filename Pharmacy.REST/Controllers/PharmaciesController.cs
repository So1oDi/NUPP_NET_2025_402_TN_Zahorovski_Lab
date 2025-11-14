using Microsoft.AspNetCore.Mvc;
using PharmacyApp.Infrastructure.Models;
using PharmacyApp.Infrastructure.Services;
using Pharmacy.REST.Models;

namespace Pharmacy.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PharmaciesController : ControllerBase
{
    private readonly ICrudServiceAsync<PharmacyModel> _pharmacyService;
    private readonly ILogger<PharmaciesController> _logger;

    public PharmaciesController(
        ICrudServiceAsync<PharmacyModel> pharmacyService,
        ILogger<PharmaciesController> logger)
    {
        _pharmacyService = pharmacyService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PharmacyDto>>> GetPharmacies()
    {
        try
        {
            var pharmacies = await _pharmacyService.ReadAllAsync();
            var result = pharmacies.Select(p => new PharmacyDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                ContactCustomerId = p.ContactCustomerId
            });
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all pharmacies");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PharmacyDto>> GetPharmacy(int id)
    {
        try
        {
            var pharmacy = await _pharmacyService.ReadAsync(id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            var result = new PharmacyDto
            {
                Id = pharmacy.Id,
                Name = pharmacy.Name,
                Address = pharmacy.Address,
                ContactCustomerId = pharmacy.ContactCustomerId
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pharmacy with ID {PharmacyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<PharmacyDto>> CreatePharmacy(CreatePharmacyDto createDto)
    {
        try
        {
            var pharmacy = new PharmacyModel
            {
                Name = createDto.Name,
                Address = createDto.Address,
                ContactCustomerId = createDto.ContactCustomerId
            };

            var created = await _pharmacyService.CreateAsync(pharmacy);
            if (!created)
            {
                return BadRequest("Failed to create pharmacy");
            }

            var result = new PharmacyDto
            {
                Id = pharmacy.Id,
                Name = pharmacy.Name,
                Address = pharmacy.Address,
                ContactCustomerId = pharmacy.ContactCustomerId
            };

            return CreatedAtAction(nameof(GetPharmacy), new { id = pharmacy.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating pharmacy");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePharmacy(int id, UpdatePharmacyDto updateDto)
    {
        try
        {
            var existingPharmacy = await _pharmacyService.ReadAsync(id);
            if (existingPharmacy == null)
            {
                return NotFound();
            }

            existingPharmacy.Name = updateDto.Name;
            existingPharmacy.Address = updateDto.Address;
            existingPharmacy.ContactCustomerId = updateDto.ContactCustomerId;

            var updated = await _pharmacyService.UpdateAsync(existingPharmacy);
            if (!updated)
            {
                return BadRequest("Failed to update pharmacy");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating pharmacy with ID {PharmacyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePharmacy(int id)
    {
        try
        {
            var pharmacy = await _pharmacyService.ReadAsync(id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            var removed = await _pharmacyService.RemoveAsync(pharmacy);
            if (!removed)
            {
                return BadRequest("Failed to delete pharmacy");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting pharmacy with ID {PharmacyId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}