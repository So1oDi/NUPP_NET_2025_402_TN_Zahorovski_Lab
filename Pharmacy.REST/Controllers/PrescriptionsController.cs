using Microsoft.AspNetCore.Mvc;
using PharmacyApp.Infrastructure.Models;
using PharmacyApp.Infrastructure.Services;
using Pharmacy.REST.Models;

namespace Pharmacy.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly ICrudServiceAsync<PrescriptionModel> _prescriptionService;
    private readonly ILogger<PrescriptionsController> _logger;

    public PrescriptionsController(
        ICrudServiceAsync<PrescriptionModel> prescriptionService,
        ILogger<PrescriptionsController> logger)
    {
        _prescriptionService = prescriptionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PrescriptionDto>>> GetPrescriptions()
    {
        try
        {
            var prescriptions = await _prescriptionService.ReadAllAsync();
            var result = prescriptions.Select(p => new PrescriptionDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                QuantityInStock = p.QuantityInStock,
                DoctorName = p.DoctorName,
                PrescriptionDate = p.PrescriptionDate,
                CustomerId = p.CustomerId
            });
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all prescriptions");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PrescriptionDto>> GetPrescription(int id)
    {
        try
        {
            var prescription = await _prescriptionService.ReadAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            var result = new PrescriptionDto
            {
                Id = prescription.Id,
                Name = prescription.Name,
                Price = prescription.Price,
                QuantityInStock = prescription.QuantityInStock,
                DoctorName = prescription.DoctorName,
                PrescriptionDate = prescription.PrescriptionDate,
                CustomerId = prescription.CustomerId
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting prescription with ID {PrescriptionId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<PrescriptionDto>> CreatePrescription(CreatePrescriptionDto createDto)
    {
        try
        {
            var prescription = new PrescriptionModel
            {
                Name = createDto.Name,
                Price = createDto.Price,
                QuantityInStock = createDto.QuantityInStock,
                DoctorName = createDto.DoctorName,
                PrescriptionDate = DateTime.Now,
                CustomerId = createDto.CustomerId
            };

            var created = await _prescriptionService.CreateAsync(prescription);
            if (!created)
            {
                return BadRequest("Failed to create prescription");
            }

            var result = new PrescriptionDto
            {
                Id = prescription.Id,
                Name = prescription.Name,
                Price = prescription.Price,
                QuantityInStock = prescription.QuantityInStock,
                DoctorName = prescription.DoctorName,
                PrescriptionDate = prescription.PrescriptionDate,
                CustomerId = prescription.CustomerId
            };

            return CreatedAtAction(nameof(GetPrescription), new { id = prescription.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating prescription");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePrescription(int id, UpdatePrescriptionDto updateDto)
    {
        try
        {
            var existingPrescription = await _prescriptionService.ReadAsync(id);
            if (existingPrescription == null)
            {
                return NotFound();
            }

            existingPrescription.Name = updateDto.Name;
            existingPrescription.Price = updateDto.Price;
            existingPrescription.QuantityInStock = updateDto.QuantityInStock;
            existingPrescription.DoctorName = updateDto.DoctorName;
            existingPrescription.CustomerId = updateDto.CustomerId;

            var updated = await _prescriptionService.UpdateAsync(existingPrescription);
            if (!updated)
            {
                return BadRequest("Failed to update prescription");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating prescription with ID {PrescriptionId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrescription(int id)
    {
        try
        {
            var prescription = await _prescriptionService.ReadAsync(id);
            if (prescription == null)
            {
                return NotFound();
            }

            var removed = await _prescriptionService.RemoveAsync(prescription);
            if (!removed)
            {
                return BadRequest("Failed to delete prescription");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting prescription with ID {PrescriptionId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}