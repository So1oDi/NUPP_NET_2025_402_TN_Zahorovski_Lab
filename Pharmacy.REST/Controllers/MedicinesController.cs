using Microsoft.AspNetCore.Mvc;
using PharmacyApp.Infrastructure.Models;
using PharmacyApp.Infrastructure.Services;
using Pharmacy.REST.Models;

namespace Pharmacy.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly ICrudServiceAsync<MedicineModel> _medicineService;
    private readonly ILogger<MedicinesController> _logger;

    public MedicinesController(
        ICrudServiceAsync<MedicineModel> medicineService,
        ILogger<MedicinesController> logger)
    {
        _medicineService = medicineService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> GetMedicines()
    {
        try
        {
            var medicines = await _medicineService.ReadAllAsync();
            var result = medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Price = m.Price,
                QuantityInStock = m.QuantityInStock
            });
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all medicines");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MedicineDto>> GetMedicine(int id)
    {
        try
        {
            var medicine = await _medicineService.ReadAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            var result = new MedicineDto
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Price = medicine.Price,
                QuantityInStock = medicine.QuantityInStock
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting medicine with ID {MedicineId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<MedicineDto>> CreateMedicine(CreateMedicineDto createDto)
    {
        try
        {
            var medicine = new MedicineModel
            {
                Name = createDto.Name,
                Price = createDto.Price,
                QuantityInStock = createDto.QuantityInStock
            };

            var created = await _medicineService.CreateAsync(medicine);
            if (!created)
            {
                return BadRequest("Failed to create medicine");
            }

            var result = new MedicineDto
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Price = medicine.Price,
                QuantityInStock = medicine.QuantityInStock
            };

            return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating medicine");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMedicine(int id, UpdateMedicineDto updateDto)
    {
        try
        {
            var existingMedicine = await _medicineService.ReadAsync(id);
            if (existingMedicine == null)
            {
                return NotFound();
            }

            existingMedicine.Name = updateDto.Name;
            existingMedicine.Price = updateDto.Price;
            existingMedicine.QuantityInStock = updateDto.QuantityInStock;

            var updated = await _medicineService.UpdateAsync(existingMedicine);
            if (!updated)
            {
                return BadRequest("Failed to update medicine");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating medicine with ID {MedicineId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedicine(int id)
    {
        try
        {
            var medicine = await _medicineService.ReadAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            var removed = await _medicineService.RemoveAsync(medicine);
            if (!removed)
            {
                return BadRequest("Failed to delete medicine");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting medicine with ID {MedicineId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("paged")]
    public async Task<ActionResult<IEnumerable<MedicineDto>>> GetMedicinesPaged(
        [FromQuery] int page = 1, 
        [FromQuery] int amount = 10)
    {
        try
        {
            var medicines = await _medicineService.ReadAllAsync(page, amount);
            var result = medicines.Select(m => new MedicineDto
            {
                Id = m.Id,
                Name = m.Name,
                Price = m.Price,
                QuantityInStock = m.QuantityInStock
            });
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paged medicines");
            return StatusCode(500, "Internal server error");
        }
    }
}